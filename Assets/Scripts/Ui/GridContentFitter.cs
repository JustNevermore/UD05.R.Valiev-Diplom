using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class GridContentFitter : MonoBehaviour
    {
        [SerializeField] private int columnCount;
        
        private void Awake()
        {
            var width = Mathf.RoundToInt(GetComponent<RectTransform>().rect.width);
            var elementWidth = width / columnCount;
            GetComponent<GridLayoutGroup>().cellSize = new Vector2(elementWidth, elementWidth);
        }
    }
}