function Login() {
    // This is the login page
    return (
        <>
            <div className="my-64">
                <div className="bg-zinc-700 mx-64 p-8 rounded-lg shadow-md transition-all hover:shadow-xl">
                    <h1 className="text-center text-3xl font-bold">Login</h1>
                    <ul className="py-12 text-center">
                        <li>
                            <a className="decoration-white text-xl bg-zinc-600 rounded-md w-min p-2 m-auto transition-all hover:bg-zinc-500 ring-indigo-500 ring-offset-zinc-700 hover:ring-2 hover:ring-offset-2" href="/api/auth/login">Gmail</a>
                        </li>
                    </ul>
                </div>
            </div>
        </>
    );
}

export default Login;