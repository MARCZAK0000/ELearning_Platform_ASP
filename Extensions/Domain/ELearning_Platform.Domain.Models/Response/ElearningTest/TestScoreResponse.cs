﻿namespace ELearning_Platform.Domain.Models.Response.ElearningTest
{
    public class TestScoreResponse
    {
        public int Score { get; set; }

        public string TestID { get; set; }

        public List<bool> IsAnswerCorrectList { get; set; }
    }
}
