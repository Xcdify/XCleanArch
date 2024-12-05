package main

import (
	"ao/api"
	"ao/config"
	"ao/data/db"
	"ao/data/db/migrations"
	"ao/pkg/logging"
)

// @securityDefinitions.apikey AuthBearer
// @in header
// @name Authorization
func main() {
	cfg := config.GetConfig()

	logger := logging.NewLogger(cfg)

	// err := cache.InitRedis(cfg)
	// if err != nil {
	// 	logger.Fatal(logging.Redis, logging.Startup, err.Error(), nil)
	// }
	// defer cache.CloseRedis()

	err := db.InitDb(cfg)
	defer db.CloseDb()
	if err != nil {
		logger.Fatal(logging.Postgres, logging.Startup, err.Error(), nil)
	}
	migrations.Up_1()

	api.InitialServer(cfg)
}
