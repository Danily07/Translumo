<p align="center">
  <img width="670" src="https://github.com/Danily07/Translumo/assets/29047281/6b3e0aa5-d772-4b63-b3ea-c7af2e003f85">
</p>
<p align="center"><b>EN</b> | <a href="docs/README-RU.md"><b>RU</b></a></p>
<p align="center">Advanced screen translator. <b>Translumo</b> is able to detect and translate appearing in the selected area text in real-time (e.g. subtitles).</p>
<h1>Main features</h1>
<ul>
  <li><b>High text recognition precision</b></li>
  <p>Translumo allows to combine the usage of several OCR engines simultaneously. It uses machine learning training model for scoring each recognized result by OCR and chooses the best one.</p>
  <img width="740" src="https://github.com/Danily07/Translumo/assets/29047281/5773de86-d3f1-41e1-a493-c37c973c8b7f)">
  <li><b>Simple interface</b></li>
  The main idea was to make tool, that does not require manual adjustments for each case and convenient for everyday use. 
  <li><b>Low latency</b></li>
  There are several implemented optimizations to reduce impact on system performance and minimize latency between the moment a text appears and actual translation. 
  <li><b>Integrated modern OCR engines:</b> Tesseract 5.0, WindowsOCR, EasyOCR</li>
  <li><b>Available translators:</b> Google Translate, Yandex translate, Naver Papago, DeepL</li>
  <li><b>Available translation languages:</b> English, Russian, Japanese, Chinese (simplified), Korean</li>
</ul>
<h1>System requirements</h1>
<ul>
  <li>Windows 10 build 19041 (20H1) / Windows 11</li>
  <li>DirectX11</li>
  <li>8 GB RAM <i>(for advanced mode with EasyOCR)</i></li>
  <li>5 GB free storage space <i>(for advanced mode with EasyOCR)</i></li>
  <li>Nvidia GPU with CUDA SDK 11.8 support (GTX 7xx series or later) <i>(for advanced mode with EasyOCR)</i></li>
</ul>
<h1>How to use</h1>
<img src="https://github.com/Danily07/Translumo/blob/7f4a73ffba0e5a0090ea0bfc3d72acb99832a0f4/docs/preview-EN.gif">
<ol>
  <li>Open the Settings</li>
  <li>Select Languages->Source language and Languages->Translation language</li>
  <li>Select Text recognition->Engines (please check Usage tips for recommendation modes)</li>
  <li>Select capture area</li>
  <li>Run translation</li>
</ol> 
<h1>Usage tips</h1>
Generally, I recommend always keep Windows OCR turned on. This is the most effective OCR for the primary text detection with less impact on performance. 
<h3>Recommended combinations of OCR engines</h3>
<ul>
  <li><b>Tesseract-Windows OCR-EasyOCR</b> - advanced mod with the highest percision</li>
  <li><b>Tesseract-Windows OCR</b> - noticeably less impact on system performance. It will be enough for cases when text has simple solid background and font is quite common</li>
  <li><b>Windows OCR-EasyOCR</b> - for very specific complex cases it makes sense to disable Tesseract and avoid unnecessary text noises</li>
</ul>
<h3>Select minimum capture area</h3>
<p>It reduces chances of getting into the area random lettters from background. Also the larger frame will take longer to process.</p>
<h3>Use proxy list to avoid blocking by translation services</h1>
<p>Some translators sometimes block client for a large number of requiests. You can configure personal/shared IPv4 proxies (1-2 should be enough) on <b>Languages->Proxy tab</b>. The application will alternately use proxies for requests to reduce number from one IP address.</p>
<h3>Use Borderless/Windowed modes in games (not Fullscreen)</h3>
<p>It is necessary to display the translation window overlay correctly.</p>
<p>If the game doesn't have such mode, you can use external tools to make it borderless (e.g. <a href="https://github.com/Codeusa/Borderless-Gaming">Borderless Gaming</a>)</p>
<h1>FAQ</h3>
<h4>I got error "Failed to capture screen" or nothing happens after translation starts</h4>
<p>Make sure that target window with text is active. Also try to restart Translumo or reopen target window.</p>
<h4>I got error "Text translation is failed" some time after successful using the translation</h4>
<p>There is a high probability that translation service temporarily blocked requests from your IP. You can change translator or configure proxy list.</p>
<h4>Can't enable Windows OCR</h4>
<p>Make sure that the application is runned as an Administrator. Translumo each time tries check installed Windows language pack via PowerShell.</p>
<h4>Package downloading for EasyOCR failed</h4>
<p>Try to re-install it under VPN</p>
<h4>Hotkeys don't work</h4>
<p>Other applications may intercept specific hotkeys handling</p>
<h1>Credits</h1>
<ul>
  <li><a href="https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit">Material Design In XAML Toolkit</a></li>
  <li><a href="https://github.com/charlesw/tesseract">Tesseract .NET wrapper</a></li>
  <li><a href="https://github.com/shimat/opencvsharp">Opencvsharp</a></li>
  <li><a href="https://github.com/pythonnet/pythonnet">Python.NET</a></li>
  <li><a href="https://github.com/JaidedAI/EasyOCR">EasyOCR</a></li>
</ul>
