using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 *	
 *  Gradient Color Using BaseVertexEffect
 *
 *	by Xuanyi
 *
 */

namespace UiEffect
{
    [AddComponentMenu ("UI/Effects/Gradient Color")]
    [RequireComponent (typeof (Graphic))]
    public class GradientColor : BaseMeshEffect
    {
        public enum DIRECTION
        {
            Vertical,
            Horizontal,
            Both,
        }

        public enum BLEND_MODE
        {
            Multiply,
            Additive,
            Subtractive,
            Override,
            None
        }

        public DIRECTION direction = DIRECTION.Both;
        public BLEND_MODE blendMode = BLEND_MODE.Override;
        public Color colorTop = Color.white;
        public Color colorBottom = Color.black;
        public Color colorLeft = Color.red;
        public Color colorRight = Color.blue;

        Graphic graphic;



        public override void ModifyMesh (VertexHelper vh)
        {
            if (IsActive () == false|| vh.currentVertCount == 0) {
                return;
            }
            List<UIVertex> vList = new List<UIVertex>();
            vh.GetUIVertexStream(vList);
            float topX = 0f, topY = 0f, bottomX = 0f, bottomY = 0f;
            foreach (var vertex in vList)
            {
                topX = Mathf.Max(topX, vertex.position.x);
                topY = Mathf.Max(topY, vertex.position.y);
                bottomX = Mathf.Min(bottomX, vertex.position.x);
                bottomY = Mathf.Min(bottomY, vertex.position.y);
            }
            float width = topX - bottomX;
            float height = topY - bottomY;

            UIVertex tempVertex =new UIVertex();
            //for (int i = 0; i < vList.Count; i++)
            //{
            //    tempVertex = vList[i];
            //    byte orgAlpha = tempVertex.color.a;
            //    Color colorOrg = tempVertex.color;
            //    Color colorV = Color.Lerp(colorBottom, colorTop, (tempVertex.position.y - bottomY) / height);
            //    Color colorH = Color.Lerp(colorLeft, colorRight, (tempVertex.position.x - bottomX) / width);
            //    switch (direction)
            //    {
            //        case DIRECTION.Both:
            //            tempVertex.color = colorOrg * colorV * colorH;
            //            break;
            //        case DIRECTION.Vertical:
            //            tempVertex.color = colorOrg * colorV;
            //            break;
            //        case DIRECTION.Horizontal:
            //            tempVertex.color = colorOrg * colorH;
            //            break;
            //    }
            //    //tempVertex.color.a = orgAlpha;
            //    //vh.SetUIVertex(tempVertex,i);
            //}
            for (int i =0; i<vh.currentVertCount;i++) {
                vh.PopulateUIVertex(ref tempVertex, i);
                byte orgAlpha = tempVertex.color.a;
                Color colorOrg = tempVertex.color;
                Color colorV = Color.Lerp(colorBottom, colorTop, (tempVertex.position.y - bottomY) / height);
                Color colorH = Color.Lerp(colorLeft, colorRight, (tempVertex.position.x - bottomX) / width);
                switch (direction)
                {
                    case DIRECTION.Both:
                        tempVertex.color = colorOrg * colorV * colorH;
                        tempVertex.color = blend(colorOrg,colorV,blendMode);
                        tempVertex.color = blend(tempVertex.color, colorH, blendMode);
                        break;
                    case DIRECTION.Vertical:
                        tempVertex.color = blend(colorOrg,colorV, blendMode);
                        break;
                    case DIRECTION.Horizontal:
                        tempVertex.color = blend(colorOrg,colorH,blendMode);
                        break;
                }
                vh.SetUIVertex(tempVertex,i);
            }
        }

        /// <summary>
        /// Refresh Gradient Color on playing.
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

        public Color blend(Color src, Color blend, BLEND_MODE mode)
        {
            Color result = src;
            float orgAlpha = result.a;
            switch (mode)
            {
                case BLEND_MODE.None:
                    result = src;
                    break;
                case BLEND_MODE.Additive:
                    result = src + blend;
                    break;
                case BLEND_MODE.Multiply:
                    result = src * blend;
                    break;
                case BLEND_MODE.Override:
                    result = blend;
                    break;
                case BLEND_MODE.Subtractive:
                    result = src - blend;
                    break;
            }
            result.a = orgAlpha;
            return result;
        }
    }
}
