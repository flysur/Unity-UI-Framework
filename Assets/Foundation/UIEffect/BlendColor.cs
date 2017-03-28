using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 *	
 *  Blend Color Using BaseMeshEffect
 *
 *	by Xuanyi
 *
 */


namespace UiEffect
{
    [AddComponentMenu ("UI/Effects/Blend Color")]
    [RequireComponent (typeof (Graphic))]
    public class BlendColor : BaseMeshEffect
    {
        public enum BLEND_MODE
        {
            Multiply,
            Additive,
            Subtractive,
            Override,
            None
        }

        public BLEND_MODE blendMode = BLEND_MODE.Multiply;
        public Color color = Color.grey;

        Graphic graphic;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (IsActive () == false ) {
                return;
            }
            //List<Color> colors = new List<Color>();
            //List<UIVertex> vList = new List<UIVertex>();
            //vh.GetUIVertexStream(vList);
            //UIVertex tempVertex = vList[0];
            //for (int i = 0; i < vList.Count; i++) {
            //    tempVertex = vList[i];
            //    byte orgAlpha = tempVertex.color.a;

            //    switch (blendMode) {
            //        case BLEND_MODE.Multiply:
            //            tempVertex.color *= color;
            //            break;
            //        case BLEND_MODE.Additive:
            //            tempVertex.color += color;
            //            break;
            //        case BLEND_MODE.Subtractive:
            //            tempVertex.color -= color;
            //            break;
            //        case BLEND_MODE.Override:
            //            tempVertex.color = color;
            //            break;
            //    }
            //    tempVertex.color.a = orgAlpha;
            //    vh.SetUIVertex(tempVertex,i);
            //}
            UIVertex tempVertex = new UIVertex();
            for (int i=0; i<vh.currentVertCount;++i) {
                vh.PopulateUIVertex(ref tempVertex,i);
                byte orgAlpha = tempVertex.color.a;
                tempVertex.color = blend(tempVertex.color,color, blendMode);
                //tempVertex.color.a = orgAlpha;
                vh.SetUIVertex(tempVertex, i);
            }
        }

        /// <summary>
        /// Refresh Blend Color on playing.
        /// </summary>
        public void Refresh ()
        {
            if (graphic == null) {
                graphic = GetComponent<Graphic> ();
            }
            if (graphic != null) {
                graphic.SetVerticesDirty ();
            }
        }

        public Color blend(Color org, Color blend, BLEND_MODE mode) {
            Color result = org;
            float orgAlpha = org.a;
            switch (mode) {
                case BLEND_MODE.None:
                    result = org;
                    break;
                case BLEND_MODE.Additive:
                    result = org + blend;
                    break;
                case BLEND_MODE.Multiply:
                    result = org * blend;
                    break;
                case BLEND_MODE.Override:
                    result = blend;
                    break;
                case BLEND_MODE.Subtractive:
                    result = org - blend;
                    break;
            }
            result.a = orgAlpha;
            return result;

        }
    }
}
