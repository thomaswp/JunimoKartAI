namespace JunimoKart
{
    public class ICue
    {
        public void Play() { }
        public void Pause() { }

        public void Resume() { }

        public bool IsPaused {  get { return false; } }
    }
}