using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TransitionCheck : StateController
{

    void UpdateMoves(string lastMove, string move);

}
