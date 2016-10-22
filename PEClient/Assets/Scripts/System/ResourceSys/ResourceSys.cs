/************************************************************
    File      : ResourceSys
	Author    : Plane
    Version   : 1.0
    Function  : Nothing
    Date      : 2016/10/20 14:40:54
    Copyright : 2016 CodeVFX Inc.
*************************************************************/

public class ResourceSys : ISystem 
{
    ///////////////////////////Data Define//////////////////////////////
    public static ResourceSys Instance = null;
    //----------------------------------------------------------------//

    ///////////////////////////MainFunctions////////////////////////////   
    public override void Init()
    {
        Instance = this;
    }

    //----------------------------------------------------------------//
}