using UnityEngine;

namespace OmiyaGames
{
    ///-----------------------------------------------------------------------
    /// <copyright file="HsvColor.cs">
    /// Code by Jonathan Czeck from Unify Community:
    /// http://wiki.unity3d.com/index.php/HSBColor
    /// 
    /// Licensed under Creative Commons Attribution-ShareAlike 3.0 Unported (CC BY-SA 3.0):
    /// http://creativecommons.org/licenses/by-sa/3.0/
    /// </copyright>
    /// <author>Jonathan Czeck</author>
    /// <author>Taro Omiya</author>
    /// <date>9/3/2015</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Displays the frame-rate in the upper-left hand corner of the screen.
    /// </summary>
    /// <remarks>
    /// Revision History:
    /// <list type="table">
    ///   <listheader>
    ///     <description>Date</description>
    ///     <description>Name</description>
    ///     <description>Description</description>
    ///   </listheader>
    ///   <item>
    ///     <description>9/3/2015</description>
    ///     <description>Jonathan Czeck</description>
    ///     <description>Wiki verison</description>
    ///   </item>
    ///   <item>
    ///     <description>3/25/2020</description>
    ///     <description>Taro Omiya</description>
    ///     <description>Converted the class to a package. Using Unity's own helper functions.</description>
    ///   </item>
    /// </list>
    /// </remarks>
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

        public HsvColor(float h, float s, float v, float a = 1f)
        {
            hue = Mathf.Clamp01(h);
            saturation = Mathf.Clamp01(s);
            value = Mathf.Clamp01(v);
            alpha = Mathf.Clamp01(a);
        }

        public HsvColor(HsvColor col) : this(col.Hue, col.Saturation, col.Value, col.Alpha) { }

        public HsvColor(Color col)
        {
            // Just use Unity's own helper function
            Color.RGBToHSV(col, out hue, out saturation, out value);
            alpha = Mathf.Clamp01(col.a);
        }

        public static HsvColor FromColor(Color color)
        {
            return new HsvColor(color);
        }

        public static Color ToColor(HsvColor color, bool isHdr = false)
        {
            // Just use Unity's helper function
            Color toReturn = Color.HSVToRGB(color.Hue, color.Saturation, color.Value, isHdr);

            // Don't forget to set the alpha value
            toReturn.a = color.Alpha;
            return toReturn;
        }

        public Color ToColor()
        {
            return ToColor(this);
        }

        public override string ToString()
        {
            return "H:" + hue + " S:" + saturation + " V:" + value;
        }

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
