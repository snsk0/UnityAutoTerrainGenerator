namespace UniPTG
{
    public interface INoiseReader
    {
        /// <summary>
        /// 指定された座標の値を返します
        /// 0～1.0fの範囲で値を返してください
        /// </summary>
        public abstract float GetValue(float x, float y);

        /// <summary>
        /// ノイズの状態を更新します
        /// </summary>
        public abstract void UpdateState();
    }
}