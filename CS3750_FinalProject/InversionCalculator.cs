using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3750_FinalProject
{
    public static class InversionCalculator
    {
        public static void CalcInversion(List<College> colleges)
        {
            foreach (var college in colleges) //loop through colleges
            {
                foreach (var department in college.Departments) //loop through depts
                {
                    foreach (var inverted in department.Employees) //loop through employees
                    {
                        if (inverted.Rank == "Instr") //ignore instructors
                            continue;

                        var moneyToFix = 0;
                        var ProfInstr = false;
                        var ProfAsst = false;
                        var ProfAsco = false;
                        var AscoInstr = false;

                        foreach (var inverter in department.Employees) //loop through and compare inverted to inverter
                        {
                            if (inverted.Rank == inverter.Rank) //ignore similar ranks
                                continue;
                            switch (inverted.Rank) //compare inverted rank and ignore higher ranks
                            {
                                case "Asst" when (inverter.Rank == "Prof" || inverter.Rank == "Asso"):
                                case "Asso" when inverter.Rank == "Prof":
                                    continue;
                            }

                            if (inverted.SalaryAmount < inverter.SalaryAmount)
                            {
                                if (moneyToFix < inverter.SalaryAmount - inverted.SalaryAmount)
                                    moneyToFix = inverter.SalaryAmount - inverted.SalaryAmount;
                            } else continue;

                            switch (inverted.Rank) //compare ranks
                            {
                                case "Prof" when inverter.Rank == "Asso":
                                    department.FullLessThanAssociate += 1;
                                    break;
                                case "Prof" when inverter.Rank == "Asst":
                                    department.FullLessThanAssistant += 1;
                                    break;
                                case "Prof" when inverter.Rank == "Instr":
                                    department.FullLessThanInstructor += 1;
                                    break;
                                case "Asso" when inverter.Rank == "Asst":
                                    department.AssociateLessThanAssistant += 1;
                                    break;
                                case "Asso" when inverter.Rank == "Instr":
                                    department.AssociateLessThanInstructor += 1;
                                    break;
                                case "Asst" when inverter.Rank == "Instr":
                                    department.AssistantLessThanInstructor += 1;
                                    break;
                            }
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