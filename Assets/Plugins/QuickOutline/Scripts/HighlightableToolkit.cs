using System.Collections.Generic;
using UnityEngine;

namespace QuickOutline 
{
    /// <summary>
    /// 网格高亮工具箱
    /// </summary>
    public static class HighlightableToolkit
    {
        private static List<Outline> OLCache = new List<Outline>();
        private static HashSet<Outline> OLs = new HashSet<Outline>();
        private static HashSet<Outline> FlashOLs = new HashSet<Outline>();
        private static HashSet<Outline> OccluderOLs = new HashSet<Outline>();

        /// <summary>
        /// 开启高亮一次，使用默认发光颜色
        /// </summary>
        /// <param name="target">目标物体</param>
        public static void OpenOnceHighLight(this GameObject target)
        {
            OpenOnceHighLight(target, Color.cyan);
        }
        /// <summary>
        /// 开启高亮一次，使用指定发光颜色
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="color">发光颜色</param>
        public static void OpenOnceHighLight(this GameObject target, Color color)
        {
            if (target == null)
                return;



            target.ClearHighLightInChildren();
            target.ClearHighLightInParent();

            HighlightableObject ho = target.GetComponent<HighlightableObject>();
            if (ho == null) ho = target.AddComponent<HighlightableObject>();

            ho.OpenOnce(color);
        }

        /// <summary>
        /// 开启持续高光，使用默认发光颜色
        /// </summary>
        /// <param name="target">目标物体</param>
        public static void OpenHighLight(this GameObject target)
        {
            OpenHighLight(target, Color.cyan,2f, Outline.Mode.OutlineAll);
        }
        /// <summary>
        /// 开启持续高光，使用指定发光颜色
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="color">发光颜色</param>
        /// <param name="isImmediate">是否立即模式</param>
        public static void OpenHighLight(this GameObject target, Color color,float _OutlineWidth, Outline.Mode _OutlineMode, bool isImmediate = true)
        {
            if (target == null)
                return;


            Outline ol = target.GetComponent<Outline>();
            if (ol == null) ol = target.AddComponent<Outline>();
            ol.OutlineWidth = _OutlineWidth;
            ol.OutlineMode = _OutlineMode;
            if (OLs.Contains(ol))
                return;

            OLs.Add(ol);

            target.ClearHighLightInChildren();
            target.ClearHighLightInParent();

            ol.OpenConstant(color);
        }
        /// <summary>
        /// 关闭持续高光
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="die">是否销毁高光实例</param>
        public static void CloseHighLight(this GameObject target, bool die = false)
        {
            if (target == null)
                return;

            Outline ol = target.GetComponent<Outline>();
            if (ol == null) return;

            OLs.Remove(ol);

            ol.CloseConstant();
            if (die) ol.Die();
        }
        /// <summary>
        /// 关闭所有的持续高光
        /// </summary>
        /// <param name="die">是否销毁高光实例</param>
        public static void CloseAllHighLight(bool die = false)
        {
            foreach (var ol in OLs)
            {
                if (ol)
                {
                    ol.CloseConstant();
                    if (die) ol.Die();
                }
            }
            OLs.Clear();
        }

        /// <summary>
        /// 开启闪光，使用默认颜色和频率
        /// </summary>
        /// <param name="target">目标物体</param>
        public static void OpenFlashHighLight(this GameObject target)
        {
            OpenFlashHighLight(target, Color.red, Color.white, 2, Outline.Mode.OutlineAll);
        }
        /// <summary>
        /// 开启闪光，使用默认频率
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        public static void OpenFlashHighLight(this GameObject target, Color color1, Color color2,float _OutlineWidth, Outline.Mode _OutlineMode)
        {
            OpenFlashHighLight(target, color1, color2, 2, _OutlineWidth, _OutlineMode);
        }
        /// <summary>
        /// 开启闪光，使用指定频率
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        /// <param name="freq">频率</param>
        public static void OpenFlashHighLight(this GameObject target, Color color1, Color color2, float freq,float _OutlineWidth, Outline.Mode _OutlineMode)
        {
            if (target == null)
                return;


            Outline ol = target.GetComponent<Outline>();
            if (ol == null) ol = target.AddComponent<Outline>();
            ol.OutlineWidth = _OutlineWidth;
            ol.OutlineMode = _OutlineMode;
            if (FlashOLs.Contains(ol))
                return;

            FlashOLs.Add(ol);

            target.ClearHighLightInChildren();
            target.ClearHighLightInParent();

            ol.OpenFlashing(color1, color2, freq);
        }
        /// <summary>
        /// 关闭闪光
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="die">是否销毁高光实例</param>
        public static void CloseFlashHighLight(this GameObject target, bool die = false)
        {
            if (target == null)
                return;

            Outline ol = target.GetComponent<Outline>();
            if (ol == null) return;

            FlashOLs.Remove(ol);

            ol.CloseFlashing();
            if (die) ol.Die();
        }
        /// <summary>
        /// 关闭所有的闪光
        /// </summary>
        /// <param name="die">是否销毁高光实例</param>
        public static void CloseAllFlashHighLight(bool die = false)
        {
            foreach (var ol in FlashOLs)
            {
                if (ol)
                {
                    ol.CloseFlashing();
                    if (die) ol.Die();
                }
            }
            FlashOLs.Clear();
        }

        /// <summary>
        /// 开启遮光板
        /// </summary>
        /// <param name="target">目标物体</param>
        public static void OpenOccluder(this GameObject target)
        {
            if (target == null)
                return;


            Outline ol = target.GetComponent<Outline>();
            if (ol == null) ol = target.AddComponent<Outline>();

            if (OccluderOLs.Contains(ol))
                return;

            OccluderOLs.Add(ol);

            target.ClearHighLightInChildren();
            target.ClearHighLightInParent();

            ol.OpenOccluder();
        }
        /// <summary>
        /// 关闭遮光板
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="die">是否销毁高光实例</param>
        public static void CloseOccluder(this GameObject target, bool die = false)
        {
            if (target == null)
                return;

            Outline ol = target.GetComponent<Outline>();
            if (ol == null) return;

            OccluderOLs.Remove(ol);

            ol.CloseOccluder();
            if (die) ol.Die();
        }
        /// <summary>
        /// 关闭所有的遮光板
        /// </summary>
        /// <param name="die">是否销毁高光实例</param>
        public static void CloseAllOccluder(bool die = false)
        {
            foreach (var ol in OccluderOLs)
            {
                if (ol)
                {
                    ol.CloseOccluder();
                    if (die) ol.Die();
                }
            }
            OccluderOLs.Clear();
        }

        /// <summary>
        /// 清空所有子物体上的高光效果，不包括自身
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="die">是否销毁高光实例</param>
        public static void ClearHighLightInChildren(this GameObject target, bool die = false)
        {
            if (target == null)
                return;

            OLCache.Clear();
            target.transform.GetComponentsInChildren(true, OLCache);
            for (int i = 0; i < OLCache.Count; i++)
            {
                if (OLCache[i].gameObject != target)
                {
                    OLCache[i].CloseAll();
                    if (die) OLCache[i].Die();
                }
            }
        }
        /// <summary>
        /// 清空所有父物体上的高光效果，不包括自身
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="die">是否销毁高光实例</param>
        public static void ClearHighLightInParent(this GameObject target, bool die = false)
        {
            if (target == null)
                return;

            OLCache.Clear();
            target.transform.GetComponentsInParent(true, OLCache);
            for (int i = 0; i < OLCache.Count; i++)
            {
                if (OLCache[i].gameObject != target)
                {
                    OLCache[i].CloseAll();
                    if (die) OLCache[i].Die();
                }
            }
        }
    }
 }