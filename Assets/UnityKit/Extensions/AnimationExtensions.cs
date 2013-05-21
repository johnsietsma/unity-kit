using UnityEngine;

public static class AnimationExtensions
{
    public static bool Exists( this Animation anim, string animName )
    {
        return anim[animName]!=null;
    }
}

