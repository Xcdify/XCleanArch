package common

import (
	"log"
	"regexp"
)

const MobileNumberPattern string = `^01(1[0-9]|2[0-2]|3[0-9]|9[0-9])[0-9]{7}$`

func MobileNumberValidate(mobileNumber string) bool {
	res, err := regexp.MatchString(MobileNumberPattern, mobileNumber)
	if err != nil {
		log.Print(err.Error())
	}
	return res
}
