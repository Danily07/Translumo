using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Translumo.Infrastructure.Encryption
{
    public class AesEncryptionService : IEncryptionService
    {
        private const int AES_BLOCK_BYTE_SIZE = 128 / 8;

        private const int PASSWORD_SALT_BYTE_SIZE = 128 / 8;
        private const int PASSWORD_BYTE_SIZE = 256 / 8;
        private const int PASSWORD_ITERATION_COUNT = 100_000;

        private readonly Encoding _stringEncoding = Encoding.UTF8;
        private readonly RandomNumberGenerator _random = RandomNumberGenerator.Create();

        public byte[] Encrypt(Stream toEncrypt, string password)
        {
            using (var aes = Aes.Create())
            {
                var keySalt = GenerateRandomBytes(PASSWORD_SALT_BYTE_SIZE);
                var key = GetKey(password, keySalt);
                var iv = GenerateRandomBytes(AES_BLOCK_BYTE_SIZE);
                var buffer = new byte[toEncrypt.Length];

                using (var encryptor = aes.CreateEncryptor(key, iv))
                {
                    toEncrypt.Read(buffer, 0, buffer.Length);
                    var cipherText = encryptor
                        .TransformFinalBlock(buffer, 0, buffer.Length);

                    var result = MergeArrays(keySalt, iv, cipherText);
                    return result;
                }
            }
        }

        public string Decrypt(Stream encryptedData, string password)
        {
            using (var aes = Aes.Create())
            {
                var buffer = new byte[encryptedData.Length];
                encryptedData.Read(buffer, 0, buffer.Length);
                var keySalt = buffer.Take(PASSWORD_SALT_BYTE_SIZE).ToArray();
                var key = GetKey(password, keySalt);
                var iv = buffer
                    .Skip(PASSWORD_SALT_BYTE_SIZE).Take(AES_BLOCK_BYTE_SIZE).ToArray();
                var cipherText = buffer
                    .Skip(PASSWORD_SALT_BYTE_SIZE + AES_BLOCK_BYTE_SIZE).ToArray();

                using (var encryptor = aes.CreateDecryptor(key, iv))
                {
                    var decryptedBytes = encryptor
                        .TransformFinalBlock(cipherText, 0, cipherText.Length);
                    return _stringEncoding.GetString(decryptedBytes);
                }
            }
        }

        private byte[] GetKey(string password, byte[] passwordSalt)
        {
            var keyBytes = _stringEncoding.GetBytes(password);

            using (var derivator = new Rfc2898DeriveBytes(
                keyBytes, passwordSalt,
                PASSWORD_ITERATION_COUNT, HashAlgorithmName.SHA256))
            {
                return derivator.GetBytes(PASSWORD_BYTE_SIZE);
            }
        }

        private byte[] GenerateRandomBytes(int numberOfBytes)
        {
            var randomBytes = new byte[numberOfBytes];
            _random.GetBytes(randomBytes);
            return randomBytes;
        }

        private byte[] MergeArrays(params byte[][] arrays)
        {
            var merged = new byte[arrays.Sum(a => a.Length)];
            var mergeIndex = 0;
            for (int i = 0; i < arrays.GetLength(0); i++)
            {
                arrays[i].CopyTo(merged, mergeIndex);
                mergeIndex += arrays[i].Length;
            }

            return merged;
        }
    }
}
