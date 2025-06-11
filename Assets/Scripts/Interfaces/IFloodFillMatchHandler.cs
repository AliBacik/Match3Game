using System.Collections.Generic;
using UnityEngine;

public interface IFloodFillMatchHandler
{
    List<BaseFruit> GetAllConnectedFruits(BaseFruit startFruit);
}
