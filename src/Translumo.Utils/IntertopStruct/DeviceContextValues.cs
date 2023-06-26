using System;

namespace Translumo.Utils.IntertopStruct
{
    [Flags]
    public enum DeviceContextValues : uint
    {
        /// <summary>
        ///     DCX_WINDOW: Returns a DC that corresponds to the window rectangle rather
        ///     than the client rectangle.
        /// </summary>
        Window = 0x00000001,

        /// <summary>
        ///     DCX_CACHE: Returns a DC from the cache, rather than the OWNDC or CLASSDC
        ///     window. Essentially overrides CS_OWNDC and CS_CLASSDC.
        /// </summary>
        Cache = 0x00000002,

        /// <summary>
        ///     DCX_NORESETATTRS: Does not reset the attributes of this DC to the
        ///     default attributes when this DC is released.
        /// </summary>
        NoResetAttrs = 0x00000004,

        /// <summary>
        ///     DCX_CLIPCHILDREN: Excludes the visible regions of all child windows
        ///     below the window identified by hWnd.
        /// </summary>
        ClipChildren = 0x00000008,

        /// <summary>
        ///     DCX_CLIPSIBLINGS: Excludes the visible regions of all sibling windows
        ///     above the window identified by hWnd.
        /// </summary>
        ClipSiblings = 0x00000010,

        /// <summary>
        ///     DCX_PARENTCLIP: Uses the visible region of the parent window. The
        ///     parent's WS_CLIPCHILDREN and CS_PARENTDC style bits are ignored. The origin is
        ///     set to the upper-left corner of the window identified by hWnd.
        /// </summary>
        ParentClip = 0x00000020,

        /// <summary>
        ///     DCX_EXCLUDERGN: The clipping region identified by hrgnClip is excluded
        ///     from the visible region of the returned DC.
        /// </summary>
        ExcludeRgn = 0x00000040,

        /// <summary>
        ///     DCX_INTERSECTRGN: The clipping region identified by hrgnClip is
        ///     intersected with the visible region of the returned DC.
        /// </summary>
        IntersectRgn = 0x00000080,

        /// <summary>DCX_EXCLUDEUPDATE: Unknown...Undocumented</summary>
        ExcludeUpdate = 0x00000100,

        /// <summary>DCX_INTERSECTUPDATE: Unknown...Undocumented</summary>
        IntersectUpdate = 0x00000200,

        /// <summary>
        ///     DCX_LOCKWINDOWUPDATE: Allows drawing even if there is a LockWindowUpdate
        ///     call in effect that would otherwise exclude this window. Used for drawing during
        ///     tracking.
        /// </summary>
        LockWindowUpdate = 0x00000400,

        /// <summary>
        ///     DCX_VALIDATE When specified with DCX_INTERSECTUPDATE, causes the DC to
        ///     be completely validated. Using this function with both DCX_INTERSECTUPDATE and
        ///     DCX_VALIDATE is identical to using the BeginPaint function.
        /// </summary>
        Validate = 0x00200000
    }
}
