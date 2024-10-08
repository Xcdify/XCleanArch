package middlewares

import (
	"net/http"

	"github.com/gin-gonic/gin"
)

func TestMiddleware() gin.HandlerFunc {
	return func(ctx *gin.Context) {
		apiKey := ctx.GetHeader("x-api-Key")
		if apiKey == "1" {
			ctx.Next()
			return
		}
		ctx.AbortWithStatusJSON(http.StatusUnauthorized, gin.H{
			"result": "Api key is required.",
		})
		return
	}
}
