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

        public int AmountToFix { get; set; }
        public int AssistantLessThanInstructor { get; set; }
        public int AssociateLessThanInstructor { get; set; }
        public int FullLessThanInstructor { get; set; }
        public int AssociateLessThanAssistant { get; set; }
        public int FullLessThanAssistant { get; set; }
        public int FullLessThanAssociate { get; set; }

        #endregion

        public InversionData()
        {
            AmountToFix = 0;
            AssistantLessThanInstructor = 0;
            AssociateLessThanInstructor = 0;
            FullLessThanInstructor = 0;
            AssociateLessThanAssistant = 0;
            FullLessThanAssistant = 0;
            FullLessThanAssociate = 0;
        }
    }
}
