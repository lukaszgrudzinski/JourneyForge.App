﻿namespace Shared
{
    public class Gothic3Quest
    {
        public string Title { get; set; } = "Empty title";
        public string Decription { get; set; } = "Empty description";
        public string? ExtraBenefits { get; set; }
        public int Exp { get; set; }
        public int ReputationValue { get; set; }
        public Gothic3Reputation ReputationType { get; set; }
        public bool IsCompleted { get; set; }
        public QuestStatus Status
        {
            get
            {
                if (IsCompleted)
                    return QuestStatus.Done;

                if (ReputationType == Gothic3Reputation.City)
                    return QuestStatus.Critical;

                return QuestStatus.NotCritical;
            }
        }
    }
}
