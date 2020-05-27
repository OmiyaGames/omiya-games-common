using UnityEngine;
using System;

namespace OmiyaGames
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="SupportedPlatformsHelpers.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2020 Omiya Games
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in
    /// all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// </copyright>
    /// <list type="table">
    /// <listheader>
    /// <term>Revision</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.4-preview.1<br/>
    /// <strong>Date:</strong> 5/26/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Initial version.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// A class full of helper and extended methods for <see cref="SupportPlatforms"/>.
    /// </summary>
    public static class SupportedPlatformsHelpers
    {
        /// <summary>
        /// 
        /// </summary>
        public class PlatformSupportArgs : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            public RuntimePlatform BuildPlatform
            {
                get;
            }

            /// <summary>
            /// 
            /// </summary>
            public bool IsSupported
            {
                get;
                set;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="currentPlatform"></param>
            /// <param name="returnFlag"></param>
            public PlatformSupportArgs(RuntimePlatform currentPlatform, bool returnFlag)
            {
                BuildPlatform = currentPlatform;
                IsSupported = returnFlag;
            }
        }

        /// <summary>
        /// 
        /// <seealso cref="PlatformSupportArgs"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public delegate void OverridePlatformSupport(SupportedPlatforms source, PlatformSupportArgs args);
        /// <summary>
        /// 
        /// </summary>
        public static event OverridePlatformSupport OnAfterIsSupportedNoArgs;

        /// <summary>
        /// Gets the number of flags in <code>SupportPlatforms</code>.
        /// It is highly recommended to cache this value.
        /// </summary>
        public static int NumberOfPlatforms
        {
            get
            {
                int returnNumber = 0;
                int flags = (int)SupportedPlatforms.AllPlatforms;
                while (flags != 0)
                {
                    // Remove the last bit
                    flags &= (flags - (1 << 0));

                    // Increment the return value;
                    ++returnNumber;
                }
                return returnNumber;
            }
        }

        /// <summary>
        /// Gets a list of all the platform names.
        /// It is highly recommended to cache this value.
        /// </summary>
        public static string[] AllPlatformNames
        {
            get
            {
                // Setup return value
                int numberOfPlatforms = NumberOfPlatforms;
                string[] returnNames = new string[numberOfPlatforms];

                // Iterate through all the platforms, in order
                SupportedPlatforms convertedEnum;
                for (int bitPosition = 0; bitPosition < numberOfPlatforms; ++bitPosition)
                {
                    convertedEnum = (SupportedPlatforms)(1 << bitPosition);
                    returnNames[bitPosition] = convertedEnum.ToString();
                }
                return returnNames;
            }
        }

        /// <summary>
        /// 
        /// <seealso cref="OnAfterIsSupportedNoArgs"/>
        /// </summary>
        /// <param name="currentPlatforms"></param>
        /// <returns></returns>
        public static bool IsSupported(this SupportedPlatforms currentPlatforms)
        {
            // Call the default method
            bool returnFlag = IsSupported(currentPlatforms, Application.platform);

            // Run an event to see if any intends to change the default return value.
            PlatformSupportArgs eventArgs = new PlatformSupportArgs(Application.platform, returnFlag);
            OnAfterIsSupportedNoArgs?.Invoke(currentPlatforms, eventArgs);

            // Return the event's aggregate return value.
            return eventArgs.IsSupported;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPlatforms"></param>
        /// <param name="singlePlatform"></param>
        /// <returns></returns>
        public static bool IsSupported(this SupportedPlatforms currentPlatforms, SupportedPlatforms singlePlatform)
        {
            return (currentPlatforms & singlePlatform) != 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPlatforms"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static bool IsSupported(this SupportedPlatforms currentPlatforms, RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return IsSupported(currentPlatforms, SupportedPlatforms.Windows);
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return IsSupported(currentPlatforms, SupportedPlatforms.MacOS);
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    return IsSupported(currentPlatforms, SupportedPlatforms.Linux);
                case RuntimePlatform.WebGLPlayer:
                    return IsSupported(currentPlatforms, SupportedPlatforms.Web);
                case RuntimePlatform.IPhonePlayer:
                    return IsSupported(currentPlatforms, SupportedPlatforms.iOS);
                case RuntimePlatform.Android:
                    return IsSupported(currentPlatforms, SupportedPlatforms.Android);
                default:
                    return false;
            }
        }
    }
}
