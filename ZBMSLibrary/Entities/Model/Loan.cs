﻿using SQLite;
using System;
using ZBMSLibrary.Entities.Enums;

namespace ZBMSLibrary.Entities.Model
{
    public abstract class Loan
    {
        [PrimaryKey]
        public string AccountNumber { get; set; }
        public string IfscCode { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public double Due { get; set; }
        public double FineAmount { get; set; }
        public double InterestRate { get; set; }
    }
}