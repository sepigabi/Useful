/**
 * Field    Identifying         Mandatory       Type    All 0 allowed   Position (without separators)   Description             validations
 * 
 * Field1   BranchCode          yes             [0-9]   no              1-7                             1-3: bank giro code     check_digit{Field1}
 *          CheckDigit (c1)                     [0-9]                   8                               4-7: branch giro code
 * 
 * 
 * Field2   Account Number 1    yes             [0-9]   conditionally,  9-15                                                    check_digit{Field2}
 *          CheckDigit (c2)                     [0-9]   only if Field3  16                                                      CDV is applicable only when
 *                                                      is filled, not                                                          - Field3 is empty, or
 *                                                      with all 0                                                              - Field3 is all 0
 *                                                  
 * 
 * Field3   Account Number 2    conditionally,  [0-9]   conditionally,  17-23                                                   when Field2 is all 0, then
 *          CheckDigit (c3)     only if         [0-9]   only if Field2  24                                                      check_digit{Field3}
 *                              - Field2 is             is filled, not
 *                              omitted or              with all 0                                                              when Field2 is not all 0 then only the
 *                              - Field2 is                                                                                     24th digit is check digit, based on
 *                              all 0                                                                                           digits 9-23:
 *                                                                                                                              check_digit{concatenate(Field2,Field3)}
 *                                                                                                                              
 *                                                                                                                              CDV is not applicable when Field3 is
 *                                                                                                                              - empty or
 *                                                                                                                              - Field3 is all 0
 *                                                                                                                              
 *                                                                                                                              
 * Examples of written representation - error messages
 *        Field1
 *  Entry1      Message
 *              Required Field
 * 10002000     Invalid Format
 * 00000000     Invalid Format
 * 10002003     (none)
 *       
 *       
 * Field2       Field3      Message1        Message2
 *                          Required Field  (none)
 *              90300010    Required Field  Invalid Format
 *              00000000    Required Field  (none)
 *              88301004    Required Field  (none)
 * 75700100                 Invalid Format  (none)
 * 75700100     90300010    Invalid Format  Invalid Format
 * 75700100     00000000    Invalid Format  (none)
 * 75700100     88301004    Invalid Format  Invalid Format
 * 00000000                 (none)          Required Field
 * 00000000     90300010    (none)          Invalid Format
 * 00000000     00000000    Invalid Format  Invalid Format
 * 00000000     88301004    (none)          (none)
 * 62800022                 (none)
 * 62800022     00000000    (none)          (none)
 * 21141249     01000007    (none)          (none)
 *     
 *     
 *     
 * Check digit algorithm
 * The commercial banks use a uniform checking algorithm that has been defined for payment and clearing services in the Decree 35/2017. (XII.
 * 14.) MNB of the Governor of the National Bank of Hungary.
 * This algorithm is based on Luhn algorithm (aka Modulo 10) using weights of 9, 7, 3 and 1.
 * Function check_digit is not about generating check digit, it's aim rather to perform CDV of given number, ie validate that it was generated in
 * the proper way (Modulo 10).
 * For example, applying the algorithm on the first group of the account number 12010879:
 * 1. Sum up the products: 1x9 + 2x7 + 0x3 + 1x1 + 0x9 + 8x7 + 7x3 + 9x1 = 110
 * 2. The check digit is valid if dividing the result (1b) with 10 the remainder is 0: 110 / 10 = 11 → no remainder
 * 


/**
 * 
 * @param field1 The first section of the BankaccountNumber (usually the first eight character)
 * @param field2 The second section of the BankaccountNumber (usually the second eight character)
 * @param field3 The third section of the BankaccountNumber (usually the third eight character)
 * @returns null if BankaccountNumber is valid; collection of key-Value pairs of validation problems otherwise. The keys are : "RequiredFields" and/or "InvalidFormats". The values could be an array which could contains the followings: "field1", "field2", "field3"
 */
export function validate(field1: string, field2: string, field3: string): { [key: string]: any } {
    let result: { [key: string]: any };
    const requiredFields = [];
    const invalidFormats = [];
    if (!field1 || field1.length !== 8) {
        requiredFields.push("field1");
    }
    else {
        if (field1 === "00000000") {
            invalidFormats.push("field1");
        }
        else {
            checkDigit(field1) ? "" : invalidFormats.push("field1");
        }
    }
    if (!field3 || field3 === "00000000") {
        if (!field2 || field2.length !== 8) {
            requiredFields.push("field2");
        }
        else if (field2 === "00000000") {
            if (field3 === "00000000") {
                invalidFormats.push("field2");
                invalidFormats.push("field3");
            }
            else {
                requiredFields.push("field3");
            }
        }
        else {
            checkDigit(field2) ? "" : invalidFormats.push("field2");
        }
    }
    else {
        if (!field2) {
            requiredFields.push("field2");
        }
        if (!field2 || field2 === "00000000") {
            if (field3.length !== 8) {
                requiredFields.push("field3");
            }
            else {
                checkDigit(field3) ? "" : invalidFormats.push("field3");
            }
        }
        else {
            if (field2.length !== 8) {
                invalidFormats.push("field2");
            }
            else if (field3.length !== 8) {
                invalidFormats.push("field3");
            }
            else {
                checkDigit(field2 + field3) ? "" : invalidFormats.push(...["field2", "field3"]);
            }
        }

    }
    if (requiredFields.length > 0) {
        result = { "RequiredFields": requiredFields };
    }
    if (invalidFormats.length > 0) {
        if (result) {
            result["InvalidFormats"] = invalidFormats;
        }
        else {
            result = { "InvalidFormats": invalidFormats };
        }
    }

    return result;
}

function checkDigit(field: string): boolean {
    const weights = [9, 7, 3, 1];
    let sum = 0;
    for (var i = 0; i < field.length; i++) {
        sum += parseInt(field[i]) * weights[i % 4];
    }
    return sum % 10 === 0;
}
