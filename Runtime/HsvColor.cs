using UnityEngine;

namespace OmiyaGames
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="HsvColor.cs">
    /// <para>
    /// Code by Jonathan Czeck from Unify Community:
    /// http://wiki.unity3d.com/index.php/HSBColor
    /// </para><para>
    /// Licensed under Creative Commons Attribution-ShareAlike 3.0 Unported (CC BY-SA 3.0):
    /// http://creativecommons.org/licenses/by-sa/3.0/
    /// </para>
    /// </copyright>
    /// <list type="table">
    /// <listheader>
    /// <term>Revision</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>
    /// <strong>Date:</strong> 9/3/2015<br/>
    /// <strong>Author:</strong> Jonathan Czeck
    /// </term>
    /// <description>
    /// Initial wiki version.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.0-preview.1<br/>
    /// <strong>Date:</strong> 3/25/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Converted the class to a package. Using Unity's own helper functions.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.4-preview.1<br/>
    /// <strong>Date:</strong> 5/27/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Updating documentation to be compatible with DocFX.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Displays the frame-rate in the upper-left hand corner of the screen.
    /// </summary>
    [System.Serializable]
    public struct HsvColor
    {
        [Range(0f, 1f)]
        [SerializeField]
        float hue;
        [Range(0f, 1f)]
        [SerializeField]
        float saturation;
        [Range(0f, 1f)]
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("brightness")]
        float value;
        [Range(0f, 1f)]
        [SerializeField]
        float alpha;

        #region Properties
        /// <summary>
        /// The hue of the color, or it's color type.<br/>
        /// Set as a fraction between 0 and 1.
        /// </summary>
        public float Hue
        {
            get
            {
                return hue;
            }
            set
            {
                hue = Mathf.Clamp01(value);
            }
        }

        /// <summary>
        /// The saturation of the color, or its "intenseness."<br/>
        /// Set as a fraction between 0 and 1.
        /// </summary>
        public float Saturation
        {
            get
            {
                return saturation;
            }
            set
            {
                saturation = Mathf.Clamp01(value);
            }
        }

        /// <summary>
        /// The value of the color.
        /// Also known as lightness, brightness, etc.<br/>
        /// Set as a fraction between 0 and 1.
        /// </summary>
        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = Mathf.Clamp01(value);
            }
        }

        /// <summary>
        /// The alpha of the color, or its opacity.<br/>
        /// Set as a fraction between 0 and 1.
        /// </summary>
        public float Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = Mathf.Clamp01(value);
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        /// <param name="a"></param>
        public HsvColor(float h, float s, float v, float a = 1f)
        {
            hue = Mathf.Clamp01(h);
            saturation = Mathf.Clamp01(s);
            value = Mathf.Clamp01(v);
            alpha = Mathf.Clamp01(a);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        public HsvColor(HsvColor col) : this(col.Hue, col.Saturation, col.Value, col.Alpha) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        public HsvColor(Color col)
        {
            // Just use Unity's own helper function
            Color.RGBToHSV(col, out hue, out saturation, out value);
            alpha = Mathf.Clamp01(col.a);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static HsvColor FromColor(Color color)
        {
            return new HsvColor(color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="isHdr"></param>
        /// <returns></returns>
        public static Color ToColor(HsvColor color, bool isHdr = false)
        {
            // Just use Unity's helper function
            Color toReturn = Color.HSVToRGB(color.Hue, color.Saturation, color.Value, isHdr);

            // Don't forget to set the alpha value
            toReturn.a = color.Alpha;
            return toReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Color ToColor()
        {
            return ToColor(this);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "H:" + hue + " S:" + saturation + " V:" + value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static HsvColor Lerp(HsvColor a, HsvColor b, float t)
        {
            float h, s;

            //check special case black (color.b==0): interpolate neither hue nor saturation!
            //check special case grey (color.s==0): don't interpolate hue!
            if (a.value == 0)
            {
                h = b.hue;
                s = b.saturation;
            }
            else if (b.value == 0)
            {
                h = a.hue;
                s = a.saturation;
            }
            else
            {
                if (a.saturation == 0)
                {
                    h = b.hue;
                }
                else if (b.saturation == 0)
                {
                    h = a.hue;
                }
                else
                {
                    // works around bug with LerpAngle
                    float angle = Mathf.LerpAngle(a.hue * 360f, b.hue * 360f, t);
                    while (angle < 0f)
                        angle += 360f;
                    while (angle > 360f)
                        angle -= 360f;
                    h = angle / 360f;
                }
                s = Mathf.Lerp(a.saturation, b.saturation, t);
            }
            return new HsvColor(h, s, Mathf.Lerp(a.value, b.value, t), Mathf.Lerp(a.alpha, b.alpha, t));
        }
    }
}
