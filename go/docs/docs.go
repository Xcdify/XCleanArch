// Package docs Code generated by swaggo/swag. DO NOT EDIT
package docs

import "github.com/swaggo/swag"

const docTemplate = `{
    "schemes": {{ marshal .Schemes }},
    "swagger": "2.0",
    "info": {
        "description": "{{escape .Description}}",
        "title": "{{.Title}}",
        "contact": {},
        "version": "{{.Version}}"
    },
    "host": "{{.Host}}",
    "basePath": "{{.BasePath}}",
    "paths": {
        "/v1/health/": {
            "get": {
                "description": "Health Check",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "summary": "Health Check",
                "responses": {
                    "200": {
                        "description": "Success",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    },
                    "400": {
                        "description": "Failed",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    }
                }
            }
        },
        "/v1/test/binder/body": {
            "post": {
                "security": [
                    {
                        "AuthBearer": []
                    }
                ],
                "description": "BodyBinder",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Test"
                ],
                "summary": "BodyBinder",
                "parameters": [
                    {
                        "description": "person data",
                        "name": "person",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/handlers.personData"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "Success",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    },
                    "401": {
                        "description": "Failed",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    }
                }
            }
        },
        "/v1/test/users/{id}": {
            "get": {
                "description": "UserById",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Test"
                ],
                "summary": "UserById",
                "parameters": [
                    {
                        "type": "integer",
                        "description": "user id",
                        "name": "id",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {
                    "200": {
                        "description": "Success",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    },
                    "400": {
                        "description": "Failed",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    }
                }
            }
        },
        "/v1/users/login-by-mobile": {
            "post": {
                "description": "RegisterLoginByMobilePhone",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Users"
                ],
                "summary": "RegisterLoginByMobilePhone",
                "parameters": [
                    {
                        "description": "RegisterLoginByMobileRequest",
                        "name": "Request",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/dto.RegisterLoginByMobileRequest"
                        }
                    }
                ],
                "responses": {
                    "201": {
                        "description": "Success",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    },
                    "400": {
                        "description": "Failed",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    },
                    "409": {
                        "description": "Failed",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    }
                }
            }
        },
        "/v1/users/login-by-username": {
            "post": {
                "description": "LoginByUsername",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Users"
                ],
                "summary": "LoginByUsername",
                "parameters": [
                    {
                        "description": "LoginByUsernameRequest",
                        "name": "Request",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/dto.LoginByUsernameRequest"
                        }
                    }
                ],
                "responses": {
                    "201": {
                        "description": "Success",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    },
                    "400": {
                        "description": "Failed",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    },
                    "409": {
                        "description": "Failed",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    }
                }
            }
        },
        "/v1/users/register-by-username": {
            "post": {
                "description": "RegisterByUsername",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Users"
                ],
                "summary": "RegisterByUsername",
                "parameters": [
                    {
                        "description": "RegisterUserByUsernameRequest",
                        "name": "Request",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/dto.RegisterUserByUsernameRequest"
                        }
                    }
                ],
                "responses": {
                    "201": {
                        "description": "Success",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    },
                    "400": {
                        "description": "Failed",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    },
                    "409": {
                        "description": "Failed",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    }
                }
            }
        },
        "/v1/users/send-otp": {
            "post": {
                "description": "Send otp to user",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Users"
                ],
                "summary": "Send otp to user",
                "parameters": [
                    {
                        "description": "GetOtpRequest",
                        "name": "Request",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/dto.GetOtpRequest"
                        }
                    }
                ],
                "responses": {
                    "201": {
                        "description": "Success",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    },
                    "400": {
                        "description": "Failed",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    },
                    "409": {
                        "description": "Failed",
                        "schema": {
                            "$ref": "#/definitions/helper.BaseHttpResponse"
                        }
                    }
                }
            }
        }
    },
    "definitions": {
        "dto.GetOtpRequest": {
            "type": "object",
            "required": [
                "mobileNumber"
            ],
            "properties": {
                "mobileNumber": {
                    "type": "string",
                    "maxLength": 11,
                    "minLength": 11
                }
            }
        },
        "dto.LoginByUsernameRequest": {
            "type": "object",
            "required": [
                "password",
                "username"
            ],
            "properties": {
                "password": {
                    "type": "string",
                    "minLength": 6
                },
                "username": {
                    "type": "string",
                    "minLength": 5
                }
            }
        },
        "dto.RegisterLoginByMobileRequest": {
            "type": "object",
            "required": [
                "mobileNumber",
                "otp"
            ],
            "properties": {
                "mobileNumber": {
                    "type": "string",
                    "maxLength": 11,
                    "minLength": 11
                },
                "otp": {
                    "type": "string",
                    "maxLength": 6,
                    "minLength": 6
                }
            }
        },
        "dto.RegisterUserByUsernameRequest": {
            "type": "object",
            "required": [
                "firstName",
                "lastName",
                "password",
                "username"
            ],
            "properties": {
                "email": {
                    "type": "string",
                    "minLength": 6
                },
                "firstName": {
                    "type": "string",
                    "minLength": 3
                },
                "lastName": {
                    "type": "string",
                    "minLength": 6
                },
                "password": {
                    "type": "string",
                    "minLength": 6
                },
                "username": {
                    "type": "string",
                    "minLength": 5
                }
            }
        },
        "handlers.personData": {
            "type": "object",
            "required": [
                "first_name",
                "last_name",
                "password",
                "phone_number"
            ],
            "properties": {
                "first_name": {
                    "type": "string",
                    "maxLength": 10,
                    "minLength": 3
                },
                "last_name": {
                    "type": "string",
                    "maxLength": 10,
                    "minLength": 3
                },
                "password": {
                    "type": "string"
                },
                "phone_number": {
                    "type": "string",
                    "maxLength": 11,
                    "minLength": 11
                }
            }
        },
        "helper.BaseHttpResponse": {
            "type": "object",
            "properties": {
                "error": {},
                "result": {},
                "resultCode": {
                    "$ref": "#/definitions/helper.ResultCode"
                },
                "success": {
                    "type": "boolean"
                },
                "validationErrors": {
                    "type": "array",
                    "items": {
                        "$ref": "#/definitions/validations.ValidationError"
                    }
                }
            }
        },
        "helper.ResultCode": {
            "type": "integer",
            "enum": [
                0,
                40001,
                40101,
                40301,
                40401,
                42901,
                442902,
                50001,
                50002
            ],
            "x-enum-varnames": [
                "Success",
                "ValidationError",
                "AuthError",
                "ForbiddenError",
                "NotFoundError",
                "LimiterError",
                "OtpLimiterError",
                "CustomRecovery",
                "InternalError"
            ]
        },
        "validations.ValidationError": {
            "type": "object",
            "properties": {
                "message": {
                    "type": "string"
                },
                "property": {
                    "type": "string"
                },
                "tag": {
                    "type": "string"
                },
                "value": {
                    "type": "string"
                }
            }
        }
    },
    "securityDefinitions": {
        "AuthBearer": {
            "type": "apiKey",
            "name": "Authorization",
            "in": "header"
        }
    }
}`

// SwaggerInfo holds exported Swagger Info so clients can modify it
var SwaggerInfo = &swag.Spec{
	Version:          "",
	Host:             "",
	BasePath:         "",
	Schemes:          []string{},
	Title:            "",
	Description:      "",
	InfoInstanceName: "swagger",
	SwaggerTemplate:  docTemplate,
	LeftDelim:        "{{",
	RightDelim:       "}}",
}

func init() {
	swag.Register(SwaggerInfo.InstanceName(), SwaggerInfo)
}
