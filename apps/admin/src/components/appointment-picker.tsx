'use client';

import { useState, useCallback, useRef, useEffect } from 'react';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { Clock, X } from 'lucide-react';

type IncrementOption = 15 | 30 | 60 | 90 | 120;
type DurationOption = 30 | 60 | 90 | 120;

interface AppointmentPickerProps {
   onChange?: (times: { start: string | null; end: string | null }) => void;
   minTime?: string;
   maxTime?: string;
   increment?: IncrementOption;
   defaultDuration?: DurationOption;
}

const AppointmentPicker = ({
   onChange,
   minTime = '01:00 AM',
   maxTime = '01:00 PM',
   increment = 30,
   defaultDuration = 60,
}: AppointmentPickerProps) => {
   const [startTime, setStartTime] = useState<string | null>(null);
   const [endTime, setEndTime] = useState<string | null>(null);
   const [timeOptions, setTimeOptions] = useState<string[]>([]);
   const [isFirstOpen, setIsFirstOpen] = useState(true);
   const defaultFocusRef = useRef<string | null>(null);

   const parseTimeString = (timeStr: string) => {
      const [time, period] = timeStr.split(' ');
      const [hours, minutes] = time.split(':').map(Number);
      return [hours, minutes, period];
   };

   const convertToMinutes = (
      hours: number,
      minutes: number,
      period: string,
   ) => {
      let totalHours = hours;
      if (period === 'PM' && hours !== 12) totalHours += 12;
      if (period === 'AM' && hours === 12) totalHours = 0;
      return totalHours * 60 + minutes;
   };

   const parseTime = (timeStr: string) => {
      const [time, period] = timeStr.split(' ');
      const [hours, minutes] = time.split(':').map(Number);
      const isPM = period === 'PM';
      return hours + (isPM && hours !== 12 ? 12 : 0) + minutes / 60;
   };

   const calculateDuration = (start: string, end: string) => {
      const startValue = parseTime(start);
      const endValue = parseTime(end);
      const durationHours = endValue - startValue;

      const hours = Math.floor(durationHours);
      const minutes = Math.round((durationHours - hours) * 60);

      if (hours === 0) {
         return `${minutes} minutes`;
      } else if (minutes === 0) {
         return `${hours} hour${hours > 1 ? 's' : ''}`;
      } else {
         return `${hours} hour${hours > 1 ? 's' : ''} ${minutes} minutes`;
      }
   };

   const isTimeInRange = useCallback(
      (time: string) => {
         const timeValue = parseTime(time);
         const minValue = parseTime(minTime);
         const maxValue = parseTime(maxTime);
         return timeValue >= minValue && timeValue <= maxValue;
      },
      [minTime, maxTime],
   );

   const filterEndTimes = useCallback(
      (time: string) => {
         if (!startTime) return isTimeInRange(time);
         return parseTime(time) > parseTime(startTime) && isTimeInRange(time);
      },
      [startTime, isTimeInRange],
   );

   const findClosestTime = useCallback(() => {
      const now = new Date();
      const currentHours = now.getHours();
      const currentMinutes = now.getMinutes();
      const currentTimeValue = currentHours + currentMinutes / 60;

      const [minHour, minMinute, minPeriod] = parseTimeString(minTime);
      const [maxHour, maxMinute, maxPeriod] = parseTimeString(maxTime);
      const minValue =
         convertToMinutes(
            minHour as number,
            minMinute as number,
            minPeriod as string,
         ) / 60;
      const maxValue =
         convertToMinutes(
            maxHour as number,
            maxMinute as number,
            maxPeriod as string,
         ) / 60;

      // If current time is outside the allowed range, return first time slot
      if (currentTimeValue >= maxValue || currentTimeValue < minValue) {
         return timeOptions[0];
      }

      let closestTime = timeOptions[0];
      let smallestDiff = 24;

      timeOptions.forEach((timeStr) => {
         const [time, period] = timeStr.split(' ');
         const [hours, minutes] = time.split(':').map(Number);
         const isPM = period === 'PM';
         const timeValue =
            hours + (isPM && hours !== 12 ? 12 : 0) + minutes / 60;

         const diff = Math.abs(timeValue - currentTimeValue);
         if (diff < smallestDiff) {
            smallestDiff = diff;
            closestTime = timeStr;
         }
      });

      return closestTime;
   }, [timeOptions, minTime, maxTime]);

   const getTimeWithDuration = (timeStr: string) => {
      const [time, period] = timeStr.split(' ');
      const [hours, minutes] = time.split(':').map(Number);
      let newHours = hours + Math.floor(defaultDuration / 60);
      let newMinutes = minutes + (defaultDuration % 60);
      if (newMinutes >= 60) {
         newHours += 1;
         newMinutes -= 60;
      }
      let newPeriod = period;

      if (period === 'AM' && newHours === 12) {
         newPeriod = 'PM';
      } else if (period === 'PM' && newHours === 12) {
         newPeriod = 'AM';
      } else if (newHours > 12) {
         newHours -= 12;
         if (period === 'AM') newPeriod = 'PM';
      }

      return `${newHours.toString().padStart(2, '0')}:${newMinutes.toString().padStart(2, '0')} ${newPeriod}`;
   };

   useEffect(() => {
      const generateTimeOptions = () => {
         const times: string[] = [];

         // Parse minTime and maxTime
         const [minHour, minMinute, minPeriod] = parseTimeString(minTime);
         const [maxHour, maxMinute, maxPeriod] = parseTimeString(maxTime);

         // Convert to minutes since midnight
         const startMinutes = convertToMinutes(
            minHour as number,
            minMinute as number,
            minPeriod.toString(),
         );
         const endMinutes = convertToMinutes(
            maxHour as number,
            maxMinute as number,
            maxPeriod.toString(),
         );

         // Include exact end time
         for (
            let minutes = startMinutes;
            minutes <= endMinutes;
            minutes += increment
         ) {
            const hour = Math.floor(minutes / 60);
            const minute = minutes % 60;
            const period = hour >= 12 ? 'PM' : 'AM';
            const displayHour =
               hour === 0
                  ? 12
                  : hour > 12
                    ? hour - 12
                    : hour === 12
                      ? 12
                      : hour;

            const timeString = `${displayHour.toString().padStart(2, '0')}:${minute.toString().padStart(2, '0')} ${period}`;
            times.push(timeString);
         }

         return times;
      };

      setTimeOptions(generateTimeOptions());
   }, [increment, minTime, maxTime]);

   useEffect(() => {
      if (timeOptions.length > 0) {
         defaultFocusRef.current = findClosestTime();
      }
   }, [timeOptions, findClosestTime]);

   useEffect(() => {
      onChange?.({ start: startTime, end: endTime });
   }, [startTime, endTime, onChange]);

   const handleStartTimeChange = (time: string | null) => {
      setStartTime(time);
      if (!time) {
         setEndTime(null);
      } else {
         const endTime = getTimeWithDuration(time);
         setEndTime(endTime);
      }
   };

   const handleEndTimeChange = (time: string | null) => {
      setEndTime(time);
   };

   const handleClear = () => {
      setStartTime(null);
      setEndTime(null);
   };

   const TimeSelect = ({
      value,
      onChange,
      placeholder,
      disabled = false,
      filterTime,
      isStartTime,
   }: {
      value: string | null;
      onChange: (time: string | null) => void;
      placeholder: string;
      disabled?: boolean;
      filterTime?: (time: string) => boolean;
      isStartTime?: boolean;
   }) => {
      const filteredOptions =
         filterTime && !isStartTime
            ? timeOptions.filter(filterTime)
            : timeOptions;

      return (
         <Select
            value={value ?? ''}
            onValueChange={(val) => onChange(val || null)}
            disabled={disabled}
            onOpenChange={(open) => {
               if (
                  open &&
                  isFirstOpen &&
                  isStartTime &&
                  defaultFocusRef.current
               ) {
                  setIsFirstOpen(false);
                  setTimeout(() => {
                     const element = document.querySelector(
                        `[data-value="${defaultFocusRef.current}"]`,
                     );
                     if (element instanceof HTMLElement) {
                        element.focus();
                        element.scrollIntoView({ block: 'center' });
                     }
                  }, 0);
               }
            }}
         >
            <SelectTrigger
               className={`w-[120px] ${disabled ? 'opacity-50' : ''}`}
            >
               <SelectValue placeholder={placeholder} className="" />
            </SelectTrigger>
            <SelectContent className="max-h-[300px]">
               {filteredOptions.length > 0 ? (
                  filteredOptions.map((time) => (
                     <SelectItem
                        key={time}
                        value={time}
                        className=""
                        data-value={time}
                     >
                        {time}
                     </SelectItem>
                  ))
               ) : (
                  <div className="py-2 px-4 text-sm">No available times</div>
               )}
            </SelectContent>
         </Select>
      );
   };

   return (
      <div className="flex flex-col gap-2 p-4 rounded-lg">
         <div className="flex items-center gap-4">
            <Clock className="h-5 w-5 " />
            <div className="flex items-center gap-2">
               <TimeSelect
                  value={startTime}
                  onChange={handleStartTimeChange}
                  placeholder="Start"
                  isStartTime={true}
               />
               <span className="">-</span>
               <TimeSelect
                  value={endTime}
                  onChange={handleEndTimeChange}
                  placeholder="End"
                  isStartTime={false}
                  disabled={!startTime}
                  filterTime={filterEndTimes}
               />
            </div>
            {(startTime || endTime) && (
               <button onClick={handleClear} className="">
                  <X className="h-4 w-4" />
               </button>
            )}
         </div>
         {startTime && endTime && (
            <div className="text-sm pl-9">
               Duration: {calculateDuration(startTime, endTime)}
            </div>
         )}
      </div>
   );
};

export default AppointmentPicker;
