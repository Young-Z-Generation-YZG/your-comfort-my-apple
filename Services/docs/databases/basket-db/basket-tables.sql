-- public.mt_doc_shoppingcart definition
CREATE TABLE public.mt_doc_shoppingcart (
	id varchar NOT NULL,
	"data" jsonb NOT NULL,
	mt_last_modified timestamptz DEFAULT transaction_timestamp() NULL,
	mt_version uuid DEFAULT md5(random()::text || clock_timestamp()::text)::uuid NOT NULL,
	mt_dotnet_type varchar NULL,
	CONSTRAINT pkey_mt_doc_shoppingcart_id PRIMARY KEY (id)
);