using System;
using System.Collections.Generic;
using System.Linq;

namespace SIM.Models
{
    public class OperationResponse
    {
        #region Properties

        public bool Response { get; set; }

        public string Message { get; set; }

        public int ErrorCode { get; set; }

        public bool ErrorOcurred { get { return ExceptionsList.Any(); } }

        public List<String> ExceptionsList { get; set; }

        public List<String> WarningsList { get; set; }

        #endregion Properties

        public OperationResponse()
        {
            Response = false;
            Message = string.Empty;
            ExceptionsList = new List<String>();
            WarningsList = new List<String>();
            ErrorCode = 0;
        }
    }
}