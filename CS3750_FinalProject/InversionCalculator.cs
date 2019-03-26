using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3750_FinalProject
{
    public static class InversionCalculator
    {
        public static void CalcInversion(List<College> Colleges)
        {
            foreach (College college in Colleges)
            {
                foreach (Department department in college.Departments)
                {
                    foreach (Employee inverted in department.Employees)
                    {
                        if (inverted.Rank == "Instr")
                            continue;
                        int moneyToFix = 0;
                        foreach (Employee inverter in department.Employees)
                        {
                            if (inverted.Rank == inverter.Rank)
                                continue;
                            if (inverted.Rank == "Asst" && (inverter.Rank == "Prof" || inverter.Rank == "Asso"))
                                continue;
                            if (inverted.Rank == "Asso" && inverter.Rank == "Prof")
                                continue;
                            if (inverted.SalaryAmount < inverter.SalaryAmount)
                            {
                                if (moneyToFix < inverter.SalaryAmount - inverted.SalaryAmount)
                                    moneyToFix = inverter.SalaryAmount - inverted.SalaryAmount;
                            }
                            else continue;
                            if (inverted.Rank == "Prof" && inverter.Rank == "Asso") department.FullLessThanAssociate += 1;
                            else if (inverted.Rank == "Prof" && inverter.Rank == "Asst") department.FullLessThanAssistant += 1;
                            else if (inverted.Rank == "Prof" && inverter.Rank == "Instr") department.FullLessThanInstructor += 1;
                            else if (inverted.Rank == "Asso" && inverter.Rank == "Asst") department.AssociateLessThanAssistant += 1;
                            else if (inverted.Rank == "Asso" && inverter.Rank == "Instr") department.AssociateLessThanInstructor += 1;
                            else if (inverted.Rank == "Asst" && inverter.Rank == "Instr") department.AssistantLessThanInstructor += 1;
                        }
                        department.AmountToFix += moneyToFix;
                    }
                    college.AmountToFix += department.AmountToFix;
                    college.AssistantLessThanInstructor += department.AssistantLessThanInstructor;
                    college.AssociateLessThanAssistant += department.AssociateLessThanAssistant;
                    college.AssociateLessThanInstructor += department.AssociateLessThanInstructor;
                    college.FullLessThanAssistant += department.FullLessThanAssistant;
                    college.FullLessThanAssociate += department.FullLessThanAssociate;
                    college.FullLessThanInstructor += department.FullLessThanInstructor;
                }
            }
        }
    }
}