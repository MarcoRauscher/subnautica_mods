using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SharedCode.Utils
{
    class SubnauticaSceneTraversalUtils
    {
        /// <summary>
        /// Find all components of type TComponent in the same base. Does NOT work for subs.
        /// </summary>
        /// <typeparam name="TComponent">Component to look for</typeparam>
        /// <param name="source">Some GameObject, whose base should be checked.</param>
        /// <returns></returns>
        public static IEnumerable<TComponent> GetComponentsInSameBase<TComponent>(GameObject source)
        {
            GameObject theBase = source.gameObject.FindParentWithComponent<Base>();

            if (theBase == null)
                yield break;

            foreach (var component in theBase.GetComponentsInChildren<TComponent>(false))
            {
                yield return component;
            }

        }
    }
}
