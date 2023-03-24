public static class Prefs
{
    public static int LevelGun
	{
        get => CPlayerPrefs.GetInt(PrefConstants.LEVEL_GUN, 0);
        set => CPlayerPrefs.SetInt(PrefConstants.LEVEL_GUN, value);
    }
}