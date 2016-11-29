import {Injectable}     from '@angular/core';

@Injectable()
export class DateTimeHelper {

    toLocalDate(data) {
        return this.toLocalFormat(data, "date");
    }

    toLocalDateTime(data) {

        return this.toLocalFormat(data, "datetime");
       
    }

    toLocalTime(data) {

        return this.toLocalFormat(data, "time");

    }

    toLocalHour(data) {

        return this.toLocalFormat(data, "hour");
        
    }

    private toLocalFormat(data, format) {
        for (var obj in data) {
            if (data.hasOwnProperty(obj)) {
                if (typeof (data[obj]) == "object") {
                    obj = this.toLocalFormat(data[obj], format);
                } else {
                    //obj is a property


                    //check to see if property value is a datetime
                    if (typeof data[obj] == "string") {
                        if (data[obj].endsWith("Z"))
                            dtNum = Date.parse(data[obj]);
                    }
                    
                    if (isNaN(dtNum) == false) {
                        var dt = new Date(dtNum);
                        data[obj] = this.toLocal(dt, format);

                        //if (format == "date") {
                        //    data[obj] = dt.toLocaleDateString()
                        //} else if (format == "datetime") {
                        //    data[obj] = dt.toLocaleDateString() + " " + dt.toLocaleTimeString();
                        //} else if (format == "hour") {
                        //    data[obj] = dt.getHours().toString();
                        //}
                    }

                    //check to see if property key is a datetime 
                    var dtNum = Date.parse(obj);
                    if (isNaN(dtNum) == false) {
                        var dt = new Date(dtNum);
                        var newField; //= dt.toLocaleDateString();

                        newField = this.toLocal(dt, format);
                        //if (format == "date") {
                        //    newField = dt.toLocaleDateString()
                        //} else if (format == "datetime") {
                        //    newField = dt.toLocaleDateString() + " " + dt.toLocaleTimeString();
                        //} else if (format == "hour") {
                        //    newField = dt.getHours().toString();
                        //}

                        Object.defineProperty(data, newField, Object.getOwnPropertyDescriptor(data, obj));
                        delete data[obj];
                    }

                }
            }
        }

        return data
    }

    private toLocal(dt, format) {
        //var dt2 = new Date();
        if (format == "date") {
            return  dt.toLocaleDateString()
        } else if (format == "datetime") {
            return dt.toLocaleDateString() + " " + dt.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
        } else if (format == "hour") {
            return dt.getHours().toString();
        } else if (format == "time") {
            return dt.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })
        }
    }

}