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
                        var profInstr = false;
                        var profAsst = false;
                        var profAsso = false;
                        var assoInstr = false;
                        var assoAsst = false;
                        var asstInstr = false;
                        var invertedYet = false;
                        InvertedEmployee newIe = new InvertedEmployee(inverted);

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
                                newIe.AddInverter(inverter);
                                invertedYet = true;
                            }
                            else
                                newIe.AddInverter(inverter);
                            switch (inverted.Rank) //compare ranks
                            {
                                case "Prof" when inverter.Rank == "Asso":
                                    profAsso = true;
                                    break;
                                case "Prof" when inverter.Rank == "Asst":
                                    profAsst = true;
                                    break;
                                case "Prof" when inverter.Rank == "Instr":
                                    profInstr = true;
                                    break;
                                case "Asso" when inverter.Rank == "Asst":
                                    assoAsst = true;
                                    break;
                                case "Asso" when inverter.Rank == "Instr":
                                    assoInstr = true;
                                    break;
                                case "Asst" when inverter.Rank == "Instr":
                                    asstInstr = true;
                                    break;
                            }
                        }

                        if (profInstr)
                        {
                            department.FullLessThanInstructor++;
                            department.FullLessThanInstructorToFix += moneyToFix;
                            newIe.FullLessThanInstructor++;
                            newIe.FullLessThanInstructorToFix += moneyToFix;
                        }
                        else if (profAsst)
                        {
                            department.FullLessThanAssistant++;
                            department.FullLessThanAssistantToFix += moneyToFix;
                            newIe.FullLessThanAssistant++;
                            newIe.FullLessThanAssistantToFix += moneyToFix;
                        }
                        else if (profAsso)
                        {
                            department.FullLessThanAssociate++;
                            department.FullLessThanAssociateToFix += moneyToFix;
                            newIe.FullLessThanAssociate++;
                            newIe.FullLessThanAssociateToFix += moneyToFix;
                        }
                        else if (assoInstr)
                        {
                            department.AssociateLessThanInstructor++;
                            department.AssociateLessThanInstructorToFix += moneyToFix;
                            newIe.AssociateLessThanInstructor++;
                            newIe.AssociateLessThanInstructorToFix += moneyToFix;
                        }
                        else if (assoAsst)
                        {
                            department.AssociateLessThanAssistant++;
                            department.AssociateLessThanAssistantToFix += moneyToFix;
                            newIe.AssociateLessThanAssistant++;
                            newIe.AssociateLessThanAssistantToFix += moneyToFix;
                        }
                        else if (asstInstr) {
                            department.AssistantLessThanInstructor++;
                            department.AssistantLessThanInstructorToFix += moneyToFix;
                            newIe.AssistantLessThanInstructor++;
                            newIe.AssistantLessThanInstructorToFix += moneyToFix;
                        }
                       
                        if (invertedYet)
                        {
                            department.TotalAmountToFix += moneyToFix;
                            newIe.TotalAmountToFix += moneyToFix;
                            department.InvertedEmployees.Add(newIe);
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