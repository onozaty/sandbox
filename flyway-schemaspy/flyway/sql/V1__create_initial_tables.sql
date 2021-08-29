CREATE TABLE customers (
  customer_id serial PRIMARY KEY,
  name text NOT NULL,
  mail_address text NOT NULL
);

COMMENT ON TABLE customers IS '顧客テーブル';
COMMENT ON COLUMN customers.customer_id IS '顧客ID';
COMMENT ON COLUMN customers.name IS '顧客名';
COMMENT ON COLUMN customers.mail_address IS 'メールアドレス';

CREATE TABLE products (
  product_id serial PRIMARY KEY,
  name text NOT NULL
);

COMMENT ON TABLE products IS '製品テーブル';
COMMENT ON COLUMN products.product_id IS '製品ID';
COMMENT ON COLUMN products.name IS '製品名';

CREATE TABLE orders (
  order_id serial PRIMARY KEY,
  customer_id integer NOT NULL REFERENCES customers(customer_id),
  date timestamp with time zone NOT NULL
);

COMMENT ON TABLE orders IS '注文テーブル';
COMMENT ON COLUMN orders.order_id IS '注文ID';
COMMENT ON COLUMN orders.customer_id IS '顧客ID';
COMMENT ON COLUMN orders.date IS '注文日時';

CREATE TABLE order_items (
  order_item_id serial PRIMARY KEY,
  order_id integer NOT NULL REFERENCES orders(order_id),
  product_id integer NOT NULL REFERENCES products(product_id),
  UNIQUE (order_id, product_id)
);

COMMENT ON TABLE order_items IS '注文製品テーブル';
COMMENT ON COLUMN order_items.order_item_id IS '注文製品ID';
COMMENT ON COLUMN order_items.order_id IS '注文ID';
COMMENT ON COLUMN order_items.product_id IS '製品ID';
