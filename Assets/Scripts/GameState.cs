public enum GameState
{
    Debug,        //AR時にエディターでデバッグを行う際に適用
    Tracking,　　　//ARトラッキング中
    Wait,         //トラッキング完了後、ゲームの準備
    Play_Move,    //移動中
    Play_Misson,  //ミッション中
    GameUp,       //ゲーム終了(ゲームオーバー、ゲームクリア)
}