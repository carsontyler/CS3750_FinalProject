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
                        var ProfAsso = false;
                        var AssoInstr = false;
                        var AssoAsst = false;
                        var AsstInstr = false;
                        var invertedYet = false;
                        InvertedEmployee newIE = new InvertedEmployee(inverted);

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
                            
                            if (inverted.SalaryAmount < inverter.SalaryAmount)//Check for salary difference
                            {
                                if (moneyToFix < inverter.SalaryAmount - inverted.SalaryAmount)
                                    moneyToFix = inverter.SalaryAmount - inverted.SalaryAmount;
                            }
                            else
                                continue;
                            if (!invertedYet) 
                            {
                                newIE.AddInverter(inverter);
                                invertedYet = true;
                            }
                            else
                                newIE.AddInverter(inverter);
                            switch (inverted.Rank) //compare ranks
                            {
                                case "Prof" when inverter.Rank == "Asso":
                                    ProfAsso = true;
                                    break;
                                case "Prof" when inverter.Rank == "Asst":
                                    ProfAsst = true;
                                    break;
                                case "Prof" when inverter.Rank == "Instr":
                                    ProfInstr = true;
                                    break;
                                case "Asso" when inverter.Rank == "Asst":
                                    AssoAsst = true;
                                    break;
                                case "Asso" when inverter.Rank == "Instr":
                                    AssoInstr = true;
                                    break;
                                case "Asst" when inverter.Rank == "Instr":
                                    AsstInstr = true;
                                    break;
                            }
                        }
                        if (ProfInstr)
                        {
                            department.FullLessThanInstructor++;
                            department.FullLessThanInstructorToFix += moneyToFix;
                            newIE.FullLessThanInstructor++;
                            newIE.FullLessThanInstructorToFix += moneyToFix;
                        }
                        else if (ProfAsst)
                        {
                            department.FullLessThanAssistant++;
                            department.FullLessThanAssistantToFix += moneyToFix;
                            newIE.FullLessThanAssistant++;
                            newIE.FullLessThanAssistantToFix += moneyToFix;
                        }
                        else if (ProfAsso)
                        {
                            department.FullLessThanAssociate++;
                            department.FullLessThanAssociateToFix += moneyToFix;
                            newIE.FullLessThanAssociate++;
                            newIE.FullLessThanAssociateToFix += moneyToFix;
                        }
                        else if (AssoInstr)
                        {
                            department.AssociateLessThanInstructor++;
                            department.AssociateLessThanInstructorToFix += moneyToFix;
                            newIE.AssociateLessThanInstructor++;
                            newIE.AssociateLessThanInstructorToFix += moneyToFix;
                        }
                        else if (AssoAsst)
                        {
                            department.AssociateLessThanAssistant++;
                            department.AssociateLessThanAssistantToFix += moneyToFix;
                            newIE.AssociateLessThanAssistant++;
                            newIE.AssociateLessThanAssistantToFix += moneyToFix;
                        }
                        else if (AsstInstr) {
                            department.AssistantLessThanInstructor++;
                            department.AssistantLessThanInstructorToFix += moneyToFix;
                            newIE.AssistantLessThanInstructor++;
                            newIE.AssistantLessThanInstructorToFix += moneyToFix;
                        }
                       
                        if (invertedYet)
                        {
                            department.TotalAmountToFix += moneyToFix;
                            newIE.TotalAmountToFix += moneyToFix;
                            department.InvertedEmployees.Add(newIE);
                        }
                    }
                    //Update College values
                    college.TotalAmountToFix += department.TotalAmountToFix;
                    college.AssistantLessThanInstructor += department.AssistantLessThanInstructor;
                    college.AssociateLessThanAssistant += department.AssociateLessThanAssistant;
                    college.AssociateLessThanInstructor += department.AssociateLessThanInstructor;
                    college.FullLessThanAssistant += department.FullLessThanAssistant;
                    college.FullLessThanAssociate += department.FullLessThanAssociate;
                    college.FullLessThanInstructor += department.FullLessThanInstructor;
                    college.AssistantLessThanInstructorToFix += department.AssistantLessThanInstructorToFix;
                    college.AssociateLessThanAssistantToFix += department.AssociateLessThanAssistantToFix;
                    college.AssociateLessThanInstructorToFix += department.AssociateLessThanInstructorToFix;
                    college.FullLessThanAssistantToFix += department.FullLessThanAssistantToFix;
                    college.FullLessThanAssociateToFix += department.FullLessThanAssociateToFix;
                    college.FullLessThanInstructorToFix += department.FullLessThanInstructorToFix;
                }
            }
        }
    }
}