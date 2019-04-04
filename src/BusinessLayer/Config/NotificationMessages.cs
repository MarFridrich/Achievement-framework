namespace BusinessLayer.Config
{
    public static class NotificationMessages
    {
        public static string RewardMessage = "User {0} asked for reward to achievement {1}";
        public static string ApproveMessage = "Reward for achievement {0} was approved"; 

        public static string CreateRewardMessageString(string userName, string achievementName)
        {
            return string.Format(RewardMessage, userName, achievementName);
        }
        
        public static string CreateApproveMessage(string achievementName)
        {
            return string.Format(ApproveMessage, achievementName);
        }
    }
}