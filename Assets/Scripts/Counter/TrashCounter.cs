using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void PickAndPlace(Player player)
    {
        if (player.HasGrabbedObject())
        {
            Destroy(player.GetGrabbedObject().gameObject);
            player.ClearGrabbedObject();

        }
        else
        {
            Debug.Log("玩家手上没有可以扔掉的东西");
        }
    }
}
