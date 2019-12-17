interface Array<T> {
    distinct(): T[];
    flat(): Array<any>;
    asyncForEach(callback: (element: T, index: number, array: Array<T>) => void) : void;
}

Array.prototype.distinct = function <T>() {
    const distinctFunction = (value: T, index: number, self: Array<T>) => {
        return self.indexOf(value) === index;
    }
    return this.filter(distinctFunction);
}

Array.prototype.asyncForEach = async function <T>(callback: (elememnt: T, index: number, array: Array<T>) => void) {
    for (let index = 0; index < this.length; index++) {
        await callback(this[index], index, this);
    }
}
