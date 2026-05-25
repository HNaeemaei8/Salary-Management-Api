
namespace OvetimePolicies
{

        public static class OvertimeCalculator
        {

            public static decimal CalculateA(decimal baseSalary, int hoursWorked, int normalHours)
            {
                int overtimeHours = Math.Max(0, hoursWorked - normalHours);
                decimal hourlyRate = baseSalary / 30 / 8; 
                return overtimeHours * hourlyRate * 1.5m;
            }

            public static decimal CalculateB(decimal baseSalary, int hoursWorked, int normalHours)
            {
                int overtimeHours = Math.Max(0, hoursWorked - normalHours);
                int cappedHours = Math.Min(overtimeHours, 10); 
                decimal hourlyRate = baseSalary / 30 / 8;
                return cappedHours * hourlyRate * 1.5m;
            }

            public static decimal CalculateC(decimal baseSalary, int hoursWorked, int normalHours)
            {
                int overtimeHours = Math.Max(0, hoursWorked - normalHours);
                decimal hourlyRate = baseSalary / 30 / 8;

                if (overtimeHours <= 10)
                    return overtimeHours * hourlyRate * 1.5m;
                else
                    return (10 * hourlyRate * 1.5m) + ((overtimeHours - 10) * hourlyRate * 2.0m);
            }
        }
    }
