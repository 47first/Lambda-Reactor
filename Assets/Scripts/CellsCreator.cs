using UnityEngine;

namespace Runtime
{
    public class CellsCreator : MonoBehaviour
    {
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Transform _cellsParent;

        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        [SerializeField] private float _xOffset;
        [SerializeField] private float _yOffset;
        [SerializeField] private float _evenRowOffset;

        [ContextMenu("Generate")]
        private void Generate()
        {
            DestroyChildren();

            for (int x = 0; x < _rows; x++)
            {
                for (int y = 0; y < _columns; y++)
                {
                    var cell = Instantiate(_cellPrefab, _cellsParent);
                    cell.Position = new Vector2(x, y);
                    cell.transform.localPosition = new Vector3(x * _xOffset + (y % 2 == 0 ? _evenRowOffset : 0), y * _yOffset, 0);
                }
            }
        }

        private void DestroyChildren()
        {
            while(transform.childCount > 0)
                DestroyImmediate(transform.GetChild(0).gameObject, true);
        }
    }
}
