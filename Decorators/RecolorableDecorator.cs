using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Custom_Scenery.Decorators
{
    class RecolorableDecorator : IDecorator
    {
        private bool _recolorable;

        public RecolorableDecorator(bool recolorable)
        {
            _recolorable = recolorable;
        }

        public void Decorate(GameObject go, Dictionary<string, object> options, AssetBundle assetBundle)
        {
            if (go.GetComponent<BuildableObject>() != null && _recolorable)
            {
                CustomColors cc = go.AddComponent<CustomColors>();

                List<Color> colors = new List<Color>();

                if (options.ContainsKey("recolorableOptions"))
                {
                    Dictionary<string, object> clrs = (Dictionary <string, object>)options["recolorableOptions"];

                    colors.AddRange(clrs.Values.Select(color => MakeColor((string) color)));
                }

                cc.customColors = colors.ToArray();
				
                foreach (Material material in Resources.FindObjectsOfTypeAll<Material>())
                {
                    if (material.name == "CustomColorsDiffuse")
                    {
                        go.GetComponentInChildren<Renderer>().sharedMaterial = material;
						
						// Go through all child objects and recolor
						Renderer[] renderCollection;
						renderCollection = go.GetComponentsInChildren<Renderer>();
						
						foreach (Renderer render in renderCollection) {
							render.sharedMaterial = material;
						}

                        break;
                    }
                }
            }
        }

        private Color FromHex(string hex)
        {
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            if (hex.Length != 6) throw new Exception("Color not valid");

            return new Color(
                int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
        }
		
		private Color MakeColor(string aStr) {
			Color clr = new Color(0,0,0);
			if(aStr!=null && aStr.Length>0) {
				try {
					string str = aStr.Substring(1, aStr.Length - 1);
					clr.r = (float)System.Int32.Parse(str.Substring(0,2), 
						System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0f;
					clr.g = (float)System.Int32.Parse(str.Substring(2,2), 
						System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0f;
					clr.b = (float)System.Int32.Parse(str.Substring(4,2), 
						System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0f;
					if(str.Length==8) clr.a = System.Int32.Parse(str.Substring(6,2), 
						System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0f;
					else clr.a = 1.0f;
				} catch(Exception e) {
					Debug.Log("Could not convert "+aStr+" to Color. "+e);
					return new Color(0,0,0,0);
				}
			}
			return clr;
		}
    }
}
