[System.Serializable]
public struct DateTime
{
    // Fields
    private Month month;
    private Days day;
    private int date;
    private int year;

    private int hour;
    private int minutes;
    
    private int totalNumDays;
    private int totalNumWeeks;

    // Properties
    public Days Day => day;
    public int Date => date;
    public int Hour => hour;
    public int Minutes => minutes;
    public Month Month => month;
    public int Year => year;
    public int TotalNumDays => totalNumDays;
    public int TotalNumWeeks => totalNumWeeks;
    public int CurrentWeek => TotalNumWeeks % 16 == 0 ? 16 : totalNumWeeks % 16;

    // Constructor
    public DateTime(int date, int month, int year, int hour, int minutes)
    {
        this.day = (Days)(date % 7);
        if (day == 0) day = (Days)7;
        this.date = date;
        this.month = (Month)month;
        this.year = year;
        this.hour = hour;
        this.minutes = minutes;

        totalNumDays = 1;
        totalNumWeeks = 1;
    }

    // Time Advancement
    public void AdvanceMinutes(int minutesToAdvanceBy)
    {
        if (minutes + minutesToAdvanceBy >= 60)
        {
            minutes = (minutes + minutesToAdvanceBy) % 60;
            AdvanceHour();
        }
        else
        {
            minutes += minutesToAdvanceBy;
        }
    }

    private void AdvanceHour()
    {
        if ((hour + 1) == 24)
        {
            hour = 0;
            AdvanceDay();
        }
        else
        {
            hour ++;
        }
    }

    private void AdvanceDay()
    {
        day ++;
        if (day > (Days)7)
        {
            day = (Days)1;
            totalNumWeeks ++;
        }
        date ++;
        if (date % 31 == 0)
        {
            AdvanceMonth();
            date = 1;
        }

        totalNumDays ++;
    }

    private void AdvanceMonth()
    {
        if (Month == Month.December)
        {
            month = Month.January;
            AdvanceYear();
        }
        else month ++;
    }

    private void AdvanceYear()
    {
        date = 1;
        year ++;
    }

    // Checks
    public bool isMidnight()
    {
        return hour == 0;
    }

    public bool isHour(int _hour)
    {
        return hour == _hour;
    }

    public bool isNight()
    {
        return hour > 21 || hour < 6;
    }

    public bool isMorning()
    {
        return hour >= 6 || hour <= 12;
    }

    public bool isAfternoon()
    {
        return hour > 12 || hour <= 21;
    }

    public bool isWeekend()
    {
        return day > Days.Friday ? true : false;
    }

    public bool IsParticularDay(Days _day)
    {
        return day == _day;
    }

    public bool IsMonth(Month _month)
    {
        return month == _month;
    }

    public bool IsStartOfMonth()
    {
        return date == 1;
    }
    public bool IsStartOfMonth(Month _month)
    {
        return date == 1 && month == _month;
    }

    // Key Dates
    public DateTime NewYearsDay(int year)
    {
        if (year == 0) year = 1;
        return new DateTime(1, 0, year, 0, 0);
    }

    // To String
    public override string ToString()
    {
        return $"Date: {DateToString()} Month: {month.ToString()} Time: {TimeToString()} " +
            $"\n Total Days: {totalNumDays} | Total Weeks: {totalNumWeeks}";
    }

    public string DateToString()
    {
        int intMonth = (int)Month + 1;
        return $"{Date.ToString("D2")}/{intMonth.ToString("D2")}/{Year.ToString("D2")}";
    }

    public string TimeToString()
    {
        return $"{Hour.ToString("D2")}:{Minutes.ToString("D2")}";
    }
}
