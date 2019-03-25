using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3750_FinalProject
{
    public class InversionData
    {
        #region Properties

        protected int AmountToFix { get; set; }
        protected int AssistantLessThanInstructor { get; set; }
        protected int AssociateLessThanInstructor { get; set; }
        protected int FullLessThanInstructor { get; set; }
        protected int AssociateLessThanAssistant { get; set; }
        protected int FullLessThanAssistant { get; set; }
        protected int FullLessThanAssociate { get; set; }

        #endregion
    }
}
