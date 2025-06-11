using System.Collections;
using UnityEngine;

public interface IGridManagerGridController
{
    void EmptyGrid(int x, int y);

    void GridRemoveFromMap(Vector2Int position);

    int ReturnWidth();

    int ReturnHeight();

    BaseFruit GridGetValue(Vector2Int pos);

    IEnumerator CollapseFruits();

    IEnumerator FillEmptyGrids();

    IEnumerator CheckIfMatchPossible();

}
