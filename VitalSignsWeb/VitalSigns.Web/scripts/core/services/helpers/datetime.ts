import {Injectable}     from '@angular/core';

@Injectable()
export class DateTimeHelper {

    toLocalDate(data, properties: Array<string> = undefined) {
        return this.toLocalFormat(data, "date", properties);
    }

    toLocalDateTime(data, properties: Array<string> = undefined) {

        return this.toLocalFormat(data, "datetime", properties);
       
    }

    toLocalTime(data, properties: Array<string> = undefined) {

        return this.toLocalFormat(data, "time", properties);

    }

    toLocalHour(data, properties: Array<string> = undefined) {

        return this.toLocalFormat(data, "hour", properties);

    }

    toLocal(data, properties: Array<string> = undefined) {
        return this.toLocalFormat(data, "datetime", properties)
    }



    
    public nameToFormat: any[] = [];

    private toLocalFormat(data, format, properties: Array<string>) {
        if (properties)
            return this.toLocalFormatWithProperties(data, format, properties);
        for (var obj in data) {
            if (data.hasOwnProperty(obj)) {
                if (typeof (data[obj]) == "object") {
                    obj = this.toLocalFormat(data[obj], format, null);
                } else {
                    //obj is a property
                    var dtNum = NaN;

                    //check to see if property value is a datetime
                    if (typeof data[obj] == "string") {
                        if (data[obj].endsWith("Z"))
                            dtNum = Date.parse(data[obj]);
                    }
                    
                    if (isNaN(dtNum) == false) {
                        var dt = new Date(dtNum);
                        data[obj] = this.toLocalValue(dt, format, obj);

                    }

                    //check to see if property key is a datetime 
                    dtNum = NaN;
                    if (typeof obj == "string") {
                        if (obj.endsWith("Z"))
                            dtNum = Date.parse(obj);
                    }
                    //var dtNum = Date.parse(obj);
                    if (isNaN(dtNum) == false) {
                        var dt = new Date(dtNum);
                        var newField; //= dt.toLocaleDateString();

                        newField = this.toLocalValue(dt, format, obj);

                        Object.defineProperty(data, newField, Object.getOwnPropertyDescriptor(data, obj));
                        delete data[obj];
                    }

                }
            }
        }

        return data
    }

    private toLocalFormatWithProperties(data, format, properties: Array<string>) {
        for (var i = 0; i < data.length; i++) {
            var obj: any = data[i];
            for (var j = 0; j < properties.length; j++) {
                var prop = properties[j]
                var dtNum = NaN;

                //check to see if property value is a datetime
                if (typeof obj[prop] == "string") {
                    if (obj[prop].endsWith("Z"))
                        dtNum = Date.parse(obj[prop]);
                }

                if (isNaN(dtNum) == false) {
                    var dt = new Date(dtNum);
                    obj[prop] = this.toLocalValue(dt, format, prop);

                }
            }
        }
        return data
    }

    private toLocalValue(dt, format, obj) {
        if (obj in this.nameToFormat)
            format = this.nameToFormat[obj];

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