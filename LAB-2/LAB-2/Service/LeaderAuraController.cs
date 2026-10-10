namespace LAB_2.Service
{
    public class LeaderAuraController
    {
        public int Duration { get; }
        public int Cooldown { get; }
        public int ActiveTurnsLeft { get; private set; } = 0;
        public int CooldownTurnsLeft { get; private set; } = 0;
        public bool IsActive => ActiveTurnsLeft > 0;
        public bool IsReady => ActiveTurnsLeft == 0 && CooldownTurnsLeft == 0;

        public LeaderAuraController(int duration = 3, int cooldown = 5)
        {
            Duration = duration;
            Cooldown = cooldown;
        }
        public bool TryActivate()
        {
            if (!IsReady) return false;

            ActiveTurnsLeft = Duration;
            return true;
        }
        public void ProcessTurn()
        {
            if (ActiveTurnsLeft > 0)
            {
                ActiveTurnsLeft--;
                if (ActiveTurnsLeft == 0)
                {
                    CooldownTurnsLeft = Cooldown; 
                }
            }
            else if (CooldownTurnsLeft > 0)
            {
                CooldownTurnsLeft--;
            }
        }
    }
}