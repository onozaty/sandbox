-- PostGIS拡張機能を有効化
CREATE EXTENSION IF NOT EXISTS postgis;

-- 場所の種類を定義する enum 型
CREATE TYPE place_type AS ENUM ('city', 'park', 'lake', 'mountain');

-- 地域情報を管理するテーブル
CREATE TABLE regions (
  region_id SERIAL PRIMARY KEY,
  name TEXT NOT NULL,
  boundary GEOMETRY(Polygon, 4326),
  created_at TIMESTAMPTZ DEFAULT statement_timestamp()
);

COMMENT ON TABLE regions IS '地域情報';
COMMENT ON COLUMN regions.region_id IS '地域ID';
COMMENT ON COLUMN regions.name IS '地域名';
COMMENT ON COLUMN regions.boundary IS '地域の境界(ポリゴン)';
COMMENT ON COLUMN regions.created_at IS '作成日時';

-- 場所情報を管理するテーブル
CREATE TABLE locations (
  location_id SERIAL PRIMARY KEY,
  name TEXT NOT NULL,
  type place_type NOT NULL,
  coordinates GEOMETRY(Point, 4326) NOT NULL,
  region_id INTEGER REFERENCES regions(region_id) ON DELETE CASCADE,
  created_at TIMESTAMPTZ DEFAULT statement_timestamp()
);

COMMENT ON TABLE locations IS '場所情報';
COMMENT ON COLUMN locations.location_id IS '場所ID';
COMMENT ON COLUMN locations.name IS '場所名';
COMMENT ON COLUMN locations.type IS '場所の種類';
COMMENT ON COLUMN locations.coordinates IS '座標情報(ポイント)';
COMMENT ON COLUMN locations.region_id IS '地域ID';
COMMENT ON COLUMN locations.created_at IS '作成日時';

-- サンプルデータの挿入
-- regions にデータを挿入
INSERT INTO regions (name, boundary)
VALUES
  ('Kanto Region', ST_SetSRID(ST_MakePolygon(ST_GeomFromText('LINESTRING(138.0 35.0, 141.0 35.0, 141.0 37.0, 138.0 37.0, 138.0 35.0)')), 4326)),
  ('New York State', ST_SetSRID(ST_MakePolygon(ST_GeomFromText('LINESTRING(-79.7624 40.4961, -71.8562 40.4961, -71.8562 45.0153, -79.7624 45.0153, -79.7624 40.4961)')), 4326));

-- locations にデータを挿入(regions の region_id を使用)
INSERT INTO locations (name, type, coordinates, region_id) 
VALUES 
  ('Tokyo', 'city', ST_SetSRID(ST_MakePoint(139.6917, 35.6895), 4326), 1),
  ('Central Park', 'park', ST_SetSRID(ST_MakePoint(-73.9654, 40.7829), 4326), 2),
  ('Lake Tahoe', 'lake', ST_SetSRID(ST_MakePoint(-119.9772, 39.0968), 4326), NULL),
  ('Mount Fuji', 'mountain', ST_SetSRID(ST_MakePoint(138.7274, 35.3606), 4326), 1);
