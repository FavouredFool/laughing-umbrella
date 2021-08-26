using UnityEngine;

public class Guard : Enemy {

    #region Variables

    #endregion


    #region UnityMethods

    

    public override void getDestroyed()
    {
        Debug.Log("I HAVE DIED A TERRIBLE DEATH!");
        // Destroy Object
        Destroy(gameObject);
    }

    #endregion
}
