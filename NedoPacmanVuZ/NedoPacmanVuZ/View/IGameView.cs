namespace NedoPacmanVuZ.View
{
    internal interface IGameView
    {
        void Render(GameCore model);
        void ShowGameOver(bool isWin);
    }
}
