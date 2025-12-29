package com.test;

import com.zaxxer.hikari.HikariConfig;
import com.zaxxer.hikari.HikariDataSource;

import javax.sql.DataSource;
import java.sql.Connection;
import java.sql.SQLException;

/**
 * Database service that handles PostgreSQL database connections with HikariCP connection pooling
 */
public class DatabaseService {

    private final HikariDataSource dataSource;

    /**
     * Constructor with connection pooling for PostgreSQL
     * @param jdbcUrl PostgreSQL JDBC URL (e.g., jdbc:postgresql://localhost:5432/estore)
     * @param username Database username
     * @param password Database password
     */
    public DatabaseService(String jdbcUrl, String username, String password) {
        HikariConfig config = new HikariConfig();
        config.setJdbcUrl(jdbcUrl);
        config.setUsername(username);
        config.setPassword(password);

        // PostgreSQL-specific optimizations
        config.setDriverClassName("org.postgresql.Driver");
        config.setMaximumPoolSize(10);
        config.setMinimumIdle(5);
        config.setConnectionTimeout(30000);
        config.setIdleTimeout(600000);
        config.setMaxLifetime(1800000);
        config.setAutoCommit(true);

        // PostgreSQL connection properties
        config.addDataSourceProperty("cachePrepStmts", "true");
        config.addDataSourceProperty("prepStmtCacheSize", "250");
        config.addDataSourceProperty("prepStmtCacheSqlLimit", "2048");
        config.addDataSourceProperty("useServerPrepStmts", "true");
        config.addDataSourceProperty("ApplicationName", "EStoreApp");

        this.dataSource = new HikariDataSource(config);
    }

    /**
     * Get the DataSource for connection pooling
     * @return HikariDataSource instance
     */
    public DataSource getDataSource() {
        return dataSource;
    }

    /**
     * Create a connection to the PostgreSQL database from the connection pool
     * @return Connection object
     * @throws SQLException if a database access error occurs
     */
    public Connection getConnection() throws SQLException {
        return dataSource.getConnection();
    }

    /**
     * Test the database connection
     * @return true if connection successful, false otherwise
     */
    public boolean testConnection() {
        try (Connection conn = getConnection()) {
            return conn != null && conn.isValid(5);
        } catch (SQLException e) {
            System.err.println("Failed to connect to PostgreSQL database: " + e.getMessage());
            return false;
        }
    }

    /**
     * Close the connection pool and release resources
     */
    public void close() {
        if (dataSource != null && !dataSource.isClosed()) {
            dataSource.close();
        }
    }
}