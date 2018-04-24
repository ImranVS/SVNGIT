function EventManager () {

	this.events = {};
	
	this.on = function (eventName, fnName, fn) {
		if (this.events[eventName] === undefined) {
			this.events[eventName]= [];
		}
		this.events[eventName][fnName] = fn;
	};
	
	this.off = function (eventName, fnName) {
		
		if (this.events === undefined) return;
		
		for (var key in this.events[eventName]) {
			if (this.events[eventName].hasOwnProperty(key)) {
				delete this.events[eventName][fnName];
			}
		}
		
	};
	
	this.dispatchEvent = function (eventName) {
		
		if (this.events === undefined) return;
		
		for (var key in this.events[eventName]) {
			if (this.events[eventName].hasOwnProperty(key)) {
				this.events[eventName][key].call(this, this.events[eventName][key]);		
			}
		}
	};
	
};