public class CardDeckService : IService, IStart
{
    void IStart.GameStart()
    {
        AssetsCollection assetsCollection = Services.Get<AssetsCollection>();
    }
}